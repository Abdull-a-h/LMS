import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BookService } from '../../../core/services/book.service';
import { AuthService } from '../../../core/services/auth.service';
import { BookDetail as BookDetailModel } from '../../../core/models/book.model';

@Component({
  selector: 'app-book-detail',
  imports: [
    RouterLink,
    MatCardModule, MatButtonModule, MatIconModule, MatChipsModule, MatProgressBarModule
  ],
  templateUrl: './book-detail.html'
})
export class BookDetail implements OnInit {
  private readonly bookService = inject(BookService);
  private readonly auth = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly isLibrarian = this.auth.isLibrarian;

  readonly book = signal<BookDetailModel | null>(null);
  readonly loading = signal(false);
  readonly uploading = signal(false);

  private bookId = '';

  ngOnInit(): void {
    this.bookId = this.route.snapshot.paramMap.get('id') ?? '';
    this.load();
  }

  private load(): void {
    this.loading.set(true);
    this.bookService.getById(this.bookId).subscribe({
      next: book => {
        this.book.set(book);
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Book not found.', 'Dismiss', { duration: 4000 });
        this.router.navigate(['/books']);
      }
    });
  }

  onCoverSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    this.uploading.set(true);
    this.bookService.uploadCover(this.bookId, file).subscribe({
      next: () => {
        this.snackBar.open('Cover updated.', 'Dismiss', { duration: 3000 });
        this.uploading.set(false);
        input.value = '';
        this.load();
      },
      error: err => {
        const message = err?.error?.error ?? 'Cover upload failed.';
        this.snackBar.open(message, 'Dismiss', { duration: 5000 });
        this.uploading.set(false);
        input.value = '';
      }
    });
  }

  removeCover(): void {
    if (!confirm('Remove the cover image?')) return;
    this.bookService.deleteCover(this.bookId).subscribe({
      next: () => {
        this.snackBar.open('Cover removed.', 'Dismiss', { duration: 3000 });
        this.load();
      },
      error: () => this.snackBar.open('Could not remove cover.', 'Dismiss', { duration: 4000 })
    });
  }

  deleteBook(): void {
    const current = this.book();
    if (!current || !confirm(`Delete "${current.title}"?`)) return;

    this.bookService.delete(this.bookId).subscribe({
      next: () => {
        this.snackBar.open('Book deleted.', 'Dismiss', { duration: 3000 });
        this.router.navigate(['/books']);
      },
      error: err => {
        const message = err?.error?.error ?? 'Could not delete book.';
        this.snackBar.open(message, 'Dismiss', { duration: 5000 });
      }
    });
  }
}
