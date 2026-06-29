import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthorService } from '../../../core/services/author.service';

@Component({
  selector: 'app-author-form',
  imports: [
    ReactiveFormsModule, RouterLink,
    MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule
  ],
  templateUrl: './author-form.html'
})
export class AuthorForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authorService = inject(AuthorService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  // Set when the route carries an :id — switches the form to edit mode.
  private authorId: string | null = null;

  readonly isEdit = signal(false);
  readonly loading = signal(false);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    biography: ['', [Validators.maxLength(1000)]],
    nationality: ['', [Validators.maxLength(100)]]
  });

  ngOnInit(): void {
    this.authorId = this.route.snapshot.paramMap.get('id');
    if (this.authorId) {
      this.isEdit.set(true);
      this.loadAuthor(this.authorId);
    }
  }

  private loadAuthor(id: string): void {
    this.loading.set(true);
    this.authorService.getById(id).subscribe({
      next: author => {
        this.form.patchValue({
          name: author.name,
          biography: author.biography ?? '',
          nationality: author.nationality ?? ''
        });
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Author not found.', 'Dismiss', { duration: 4000 });
        this.router.navigate(['/authors']);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);

    const request = this.form.getRawValue();
    const op = this.isEdit() && this.authorId
      ? this.authorService.update(this.authorId, request)
      : this.authorService.create(request);

    op.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Author updated.' : 'Author created.', 'Dismiss', { duration: 3000 });
        this.router.navigate(['/authors']);
      },
      error: () => {
        this.snackBar.open('Save failed. Please check the form and try again.', 'Dismiss', { duration: 4000 });
        this.loading.set(false);
      }
    });
  }
}
