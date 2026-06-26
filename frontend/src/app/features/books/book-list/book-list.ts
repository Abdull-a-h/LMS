import { Component, inject } from '@angular/core';
import { BookService } from '../../../core/services/book.service';

@Component({
  selector: 'app-book-list',
  imports: [],
  templateUrl: './book-list.html'
})
export class BookList {
  // TODO: paginated, searchable book grid with cover thumbnails + availability.
  protected readonly books = inject(BookService);
}
