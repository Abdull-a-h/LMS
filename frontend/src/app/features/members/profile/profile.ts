import { Component, inject } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.html'
})
export class Profile {
  // TODO: current member's own profile (/members/me).
  protected readonly members = inject(MemberService);
}
