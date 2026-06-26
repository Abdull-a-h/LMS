import { Component, inject } from '@angular/core';
import { BorrowService } from '../../../core/services/borrow.service';

@Component({
  selector: 'app-my-borrows',
  imports: [],
  templateUrl: './my-borrows.html'
})
export class MyBorrows {
  // TODO: current member's borrow history with return actions + overdue badges.
  protected readonly borrows = inject(BorrowService);
}
