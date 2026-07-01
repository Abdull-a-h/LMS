import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BookService } from '../../../core/services/book.service';
import { AuthorService } from '../../../core/services/author.service';
import { Author } from '../../../core/models/author.model';

@Component({
  selector: 'app-book-form',
  imports: [
    ReactiveFormsModule, RouterLink,
    MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule
  ],
  templateUrl: './book-form.html'
})
export class BookForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly bookService = inject(BookService);
  private readonly authorService = inject(AuthorService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  private bookId: string | null = null;

  readonly isEdit = signal(false);
  readonly loading = signal(false);
  readonly authors = signal<Author[]>([]);

  readonly currentYear = new Date().getFullYear();

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(250)]],
    description: ['', [Validators.maxLength(2000)]],
    isbn: ['', [Validators.required, Validators.pattern(/^\d{13}$/)]],
    publicationYear: [this.currentYear, [Validators.required, Validators.min(1450), Validators.max(this.currentYear + 1)]],
    totalCopies: [1, [Validators.required, Validators.min(1)]],
    authorId: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.loadAuthors();

    this.bookId = this.route.snapshot.paramMap.get('id');
    if (this.bookId) {
      this.isEdit.set(true);
      this.loadBook(this.bookId);
    }
  }

  private loadAuthors(): void {
    this.authorService.getAll().subscribe({
      next: authors => this.authors.set(authors),
      error: () => this.snackBar.open('Failed to load authors.', 'Dismiss', { duration: 4000 })
    });
  }

  private loadBook(id: string): void {
    this.loading.set(true);
    this.bookService.getById(id).subscribe({
      next: book => {
        this.form.patchValue({
          title: book.title,
          description: book.description ?? '',
          isbn: book.isbn,
          publicationYear: book.publicationYear,
          totalCopies: book.totalCopies,
          authorId: book.author.id
        });
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Book not found.', 'Dismiss', { duration: 4000 });
        this.router.navigate(['/books']);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);

    const request = this.form.getRawValue();
    const op = this.isEdit() && this.bookId
      ? this.bookService.update(this.bookId, request)
      : this.bookService.create(request);

    op.subscribe({
      next: book => {
        this.snackBar.open(this.isEdit() ? 'Book updated.' : 'Book created.', 'Dismiss', { duration: 3000 });
        // Land on the detail page so the librarian can add a cover next.
        this.router.navigate(['/books', book.id]);
      },
      error: err => {
        // 422 on duplicate ISBN or copies-below-borrowed; 400 on validation.
        const message = err?.error?.error ?? err?.error?.errors?.[0]?.message ?? 'Save failed.';
        this.snackBar.open(message, 'Dismiss', { duration: 5000 });
        this.loading.set(false);
      }
    });
  }
}
