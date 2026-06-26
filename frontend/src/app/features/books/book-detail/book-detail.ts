import { Component, inject } from '@angular/core';
import { BookService } from '../../../core/services/book.service';

@Component({
  selector: 'app-book-detail',
  imports: [],
  templateUrl: './book-detail.html'
})
export class BookDetail {
  // TODO: full book detail, cover, availability, borrow/return action.
  protected readonly books = inject(BookService);
}
