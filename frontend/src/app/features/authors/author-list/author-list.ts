import { Component, inject } from '@angular/core';
import { AuthorService } from '../../../core/services/author.service';

@Component({
  selector: 'app-author-list',
  imports: [],
  templateUrl: './author-list.html'
})
export class AuthorList {
  // TODO: list authors; Librarian gets create/edit/delete actions.
  protected readonly authors = inject(AuthorService);
}
