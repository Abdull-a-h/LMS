import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResult, LoginRequest, RegisterRequest, UserRole } from '../models/auth.model';

const ACCESS_TOKEN_KEY = 'lms.accessToken';
const REFRESH_TOKEN_KEY = 'lms.refreshToken';
const ROLE_KEY = 'lms.role';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/auth`;

  private readonly accessToken = signal<string | null>(localStorage.getItem(ACCESS_TOKEN_KEY));
  readonly role = signal<UserRole | null>(localStorage.getItem(ROLE_KEY) as UserRole | null);
  readonly isAuthenticated = computed(() => !!this.accessToken());
  readonly isLibrarian = computed(() => this.role() === 'Librarian');

  register(request: RegisterRequest): Observable<AuthResult> {
    return this.http.post<AuthResult>(`${this.baseUrl}/register`, request).pipe(tap(r => this.store(r)));
  }

  login(request: LoginRequest): Observable<AuthResult> {
    return this.http.post<AuthResult>(`${this.baseUrl}/login`, request).pipe(tap(r => this.store(r)));
  }

  refresh(): Observable<AuthResult> {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    return this.http.post<AuthResult>(`${this.baseUrl}/refresh`, { refreshToken }).pipe(tap(r => this.store(r)));
  }

  logout(): Observable<void> {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    return this.http.post<void>(`${this.baseUrl}/logout`, { refreshToken }).pipe(tap(() => this.clear()));
  }

  getAccessToken(): string | null {
    return this.accessToken();
  }

  clear(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(ROLE_KEY);
    this.accessToken.set(null);
    this.role.set(null);
  }

  private store(result: AuthResult): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, result.accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, result.refreshToken);
    localStorage.setItem(ROLE_KEY, result.role);
    this.accessToken.set(result.accessToken);
    this.role.set(result.role);
  }
}
