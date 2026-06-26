export type UserRole = 'Member' | 'Librarian';

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResult {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAt: string;
  memberId: string;
  fullName: string;
  role: UserRole;
}
