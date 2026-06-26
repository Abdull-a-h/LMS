import { Component, inject } from '@angular/core';
import { AuthorService } from '../../../core/services/author.service';

@Component({
  selector: 'app-author-form',
  imports: [],
  templateUrl: './author-form.html'
})
export class AuthorForm {
  // TODO: reactive create/edit author form (Librarian).
  protected readonly authors = inject(AuthorService);
}
