import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthorService } from '../../../core/services/author.service';
import { AuthService } from '../../../core/services/auth.service';
import { Author } from '../../../core/models/author.model';

@Component({
  selector: 'app-author-list',
  imports: [RouterLink, MatTableModule, MatButtonModule, MatIconModule, MatProgressBarModule],
  templateUrl: './author-list.html'
})
export class AuthorList implements OnInit {
  private readonly authorService = inject(AuthorService);
  private readonly auth = inject(AuthService);
  private readonly snackBar = inject(MatSnackBar);

  readonly isLibrarian = this.auth.isLibrarian;

  readonly authors = signal<Author[]>([]);
  readonly loading = signal(false);

  // Columns shown to a member; the librarian additionally sees an actions column.
  protected get displayedColumns(): string[] {
    return this.isLibrarian()
      ? ['name', 'nationality', 'actions']
      : ['name', 'nationality'];
  }

  ngOnInit(): void {
    this.load();
  }

  private load(): void {
    this.loading.set(true);
    this.authorService.getAll().subscribe({
      next: authors => {
        this.authors.set(authors);
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to load authors.', 'Dismiss', { duration: 4000 });
        this.loading.set(false);
      }
    });
  }

  delete(author: Author): void {
    if (!confirm(`Delete author "${author.name}"?`)) return;

    this.authorService.delete(author.id).subscribe({
      next: () => {
        this.snackBar.open('Author deleted.', 'Dismiss', { duration: 3000 });
        this.load();
      },
      error: err => {
        // 422 from the BusinessRuleException (author still has active books).
        const message = err?.error?.error ?? 'Could not delete author.';
        this.snackBar.open(message, 'Dismiss', { duration: 5000 });
      }
    });
  }
}
