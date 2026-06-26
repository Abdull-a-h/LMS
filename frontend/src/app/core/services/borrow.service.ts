import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BorrowRecord } from '../models/borrow.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class BorrowService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/borrows`;

  borrow(bookId: string): Observable<BorrowRecord> {
    return this.http.post<BorrowRecord>(this.baseUrl, { bookId });
  }

  return(borrowRecordId: string): Observable<BorrowRecord> {
    return this.http.patch<BorrowRecord>(`${this.baseUrl}/${borrowRecordId}/return`, {});
  }

  getMyBorrows(page = 1, pageSize = 10): Observable<PagedResult<BorrowRecord>> {
    const params = new HttpParams().set('page', page).set('pageSize', pageSize);
    return this.http.get<PagedResult<BorrowRecord>>(`${this.baseUrl}/my`, { params });
  }

  getAll(page = 1, pageSize = 10): Observable<PagedResult<BorrowRecord>> {
    const params = new HttpParams().set('page', page).set('pageSize', pageSize);
    return this.http.get<PagedResult<BorrowRecord>>(this.baseUrl, { params });
  }
}
