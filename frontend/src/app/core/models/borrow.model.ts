export interface BorrowRecord {
  id: string;
  bookId: string;
  bookTitle: string;
  borrowedAt: string;
  dueDate: string;
  returnedAt?: string;
  isOverdue: boolean;
}

export interface BorrowRequest {
  bookId: string;
}
