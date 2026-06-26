import { Component, inject } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';

@Component({
  selector: 'app-member-detail',
  imports: [],
  templateUrl: './member-detail.html'
})
export class MemberDetail {
  // TODO: member profile + borrow history (Librarian).
  protected readonly members = inject(MemberService);
}
