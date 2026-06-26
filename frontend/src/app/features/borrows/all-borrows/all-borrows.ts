import { Component, inject } from '@angular/core';
import { BorrowService } from '../../../core/services/borrow.service';

@Component({
  selector: 'app-all-borrows',
  imports: [],
  templateUrl: './all-borrows.html'
})
export class AllBorrows {
  // TODO: all borrow records (Librarian), paginated.
  protected readonly borrows = inject(BorrowService);
}
