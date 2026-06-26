import { Component, inject } from '@angular/core';
import { BookService } from '../../../core/services/book.service';

@Component({
  selector: 'app-book-form',
  imports: [],
  templateUrl: './book-form.html'
})
export class BookForm {
  // TODO: reactive create/edit form (Librarian) + cover upload.
  protected readonly books = inject(BookService);
}
