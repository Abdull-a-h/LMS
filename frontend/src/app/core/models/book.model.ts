import { Author } from './author.model';

export interface BookSummary {
  id: string;
  title: string;
  isbn: string;
}

export interface Book {
  id: string;
  title: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  coverImageUrl?: string;
  authorId: string;
  authorName: string;
}

export interface BookDetail {
  id: string;
  title: string;
  description?: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  coverImageUrl?: string;
  author: Author;
  hasActiveBorrowByCurrentMember: boolean;
}

export interface BookRequest {
  title: string;
  description?: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  authorId: string;
}
