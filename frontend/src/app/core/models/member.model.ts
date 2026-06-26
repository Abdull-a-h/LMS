import { BorrowRecord } from './borrow.model';
import { UserRole } from './auth.model';

export interface MemberSummary {
  id: string;
  fullName: string;
  email: string;
  role: UserRole;
  isActive: boolean;
}

export interface MemberDetail extends MemberSummary {
  activeBorrowCount: number;
  borrowHistory: BorrowRecord[];
}
