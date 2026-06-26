import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MemberDetail, MemberSummary } from '../models/member.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class MemberService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/members`;

  getMembers(q?: string, page = 1, pageSize = 10): Observable<PagedResult<MemberSummary>> {
    let params = new HttpParams().set('page', page).set('pageSize', pageSize);
    if (q) params = params.set('q', q);
    return this.http.get<PagedResult<MemberSummary>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<MemberDetail> {
    return this.http.get<MemberDetail>(`${this.baseUrl}/${id}`);
  }

  getMyProfile(): Observable<MemberDetail> {
    return this.http.get<MemberDetail>(`${this.baseUrl}/me`);
  }

  deactivate(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
