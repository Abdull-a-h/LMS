import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Author, AuthorDetail, AuthorRequest } from '../models/author.model';

@Injectable({ providedIn: 'root' })
export class AuthorService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/authors`;

  getAll(): Observable<Author[]> {
    return this.http.get<Author[]>(this.baseUrl);
  }

  getById(id: string): Observable<AuthorDetail> {
    return this.http.get<AuthorDetail>(`${this.baseUrl}/${id}`);
  }

  create(request: AuthorRequest): Observable<Author> {
    return this.http.post<Author>(this.baseUrl, request);
  }

  update(id: string, request: AuthorRequest): Observable<Author> {
    return this.http.put<Author>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
