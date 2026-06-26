import { BookSummary } from './book.model';

export interface Author {
  id: string;
  name: string;
  biography?: string;
  nationality?: string;
}

export interface AuthorDetail extends Author {
  books: BookSummary[];
}

export interface AuthorRequest {
  name: string;
  biography?: string;
  nationality?: string;
}
