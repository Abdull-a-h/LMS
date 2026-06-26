import { Component, inject } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';

@Component({
  selector: 'app-member-list',
  imports: [],
  templateUrl: './member-list.html'
})
export class MemberList {
  // TODO: paginated, searchable members (Librarian) with deactivate action.
  protected readonly members = inject(MemberService);
}
