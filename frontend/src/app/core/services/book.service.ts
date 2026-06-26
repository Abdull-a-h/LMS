import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Book, BookDetail, BookRequest } from '../models/book.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class BookService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/books`;

  getBooks(authorId?: string, q?: string, page = 1, pageSize = 10): Observable<PagedResult<Book>> {
    let params = new HttpParams().set('page', page).set('pageSize', pageSize);
    if (authorId) params = params.set('authorId', authorId);
    if (q) params = params.set('q', q);
    return this.http.get<PagedResult<Book>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<BookDetail> {
    return this.http.get<BookDetail>(`${this.baseUrl}/${id}`);
  }

  create(request: BookRequest): Observable<Book> {
    return this.http.post<Book>(this.baseUrl, request);
  }

  update(id: string, request: BookRequest): Observable<Book> {
    return this.http.put<Book>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  uploadCover(id: string, file: File): Observable<{ coverImageUrl: string }> {
    const form = new FormData();
    form.append('file', file);
    return this.http.post<{ coverImageUrl: string }>(`${this.baseUrl}/${id}/cover`, form);
  }

  deleteCover(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}/cover`);
  }
}
