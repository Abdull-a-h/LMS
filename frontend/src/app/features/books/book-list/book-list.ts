import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BookService } from '../../../core/services/book.service';
import { AuthService } from '../../../core/services/auth.service';
import { Book } from '../../../core/models/book.model';

@Component({
  selector: 'app-book-list',
  imports: [
    FormsModule, RouterLink,
    MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
    MatIconModule, MatPaginatorModule, MatProgressBarModule, MatChipsModule
  ],
  templateUrl: './book-list.html'
})
export class BookList implements OnInit {
  private readonly bookService = inject(BookService);
  private readonly auth = inject(AuthService);
  private readonly snackBar = inject(MatSnackBar);

  readonly isLibrarian = this.auth.isLibrarian;

  readonly books = signal<Book[]>([]);
  readonly totalCount = signal(0);
  readonly loading = signal(false);

  // MatPaginator is 0-based; the API is 1-based.
  keyword = '';
  pageIndex = 0;
  pageSize = 8;

  ngOnInit(): void {
    this.load();
  }

  private load(): void {
    this.loading.set(true);
    this.bookService.getBooks(undefined, this.keyword.trim() || undefined, this.pageIndex + 1, this.pageSize)
      .subscribe({
        next: result => {
          this.books.set(result.items);
          this.totalCount.set(result.totalCount);
          this.loading.set(false);
        },
        error: () => {
          this.snackBar.open('Failed to load books.', 'Dismiss', { duration: 4000 });
          this.loading.set(false);
        }
      });
  }

  search(): void {
    this.pageIndex = 0;
    this.load();
  }

  onPage(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.load();
  }

  delete(book: Book, event: Event): void {
    event.stopPropagation();
    if (!confirm(`Delete "${book.title}"?`)) return;

    this.bookService.delete(book.id).subscribe({
      next: () => {
        this.snackBar.open('Book deleted.', 'Dismiss', { duration: 3000 });
        this.load();
      },
      error: err => {
        // 422 if the book still has active borrows.
        const message = err?.error?.error ?? 'Could not delete book.';
        this.snackBar.open(message, 'Dismiss', { duration: 5000 });
      }
    });
  }
}
