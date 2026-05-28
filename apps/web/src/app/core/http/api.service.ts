import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResult } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl.replace(/\/$/, '');

  get<T>(path: string): Observable<T> {
    return this.http.get<ApiResult<T>>(this.url(path)).pipe(map(res => this.unwrap(res)));
  }

  post<T>(path: string, body?: unknown): Observable<T> {
    return this.http.post<ApiResult<T>>(this.url(path), body).pipe(map(res => this.unwrap(res)));
  }

  put<T>(path: string, body?: unknown): Observable<T> {
    return this.http.put<ApiResult<T>>(this.url(path), body).pipe(map(res => this.unwrap(res)));
  }

  patch<T>(path: string, body?: unknown): Observable<T> {
    return this.http.patch<ApiResult<T>>(this.url(path), body).pipe(map(res => this.unwrap(res)));
  }

  delete<T>(path: string): Observable<T> {
    return this.http.delete<ApiResult<T>>(this.url(path)).pipe(map(res => this.unwrap(res)));
  }

  upload<T>(path: string, formData: FormData): Observable<T> {
    return this.http.post<ApiResult<T>>(this.url(path), formData).pipe(map(res => this.unwrap(res)));
  }

  rawGet<T>(absoluteUrl: string): Observable<T> {
    return this.http.get<T>(absoluteUrl);
  }

  rawPost<T>(absoluteUrl: string, body?: unknown): Observable<T> {
    return this.http.post<T>(absoluteUrl, body);
  }

  absolute(path: string): string {
    return this.url(path);
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path.replace(/^\//, '')}`;
  }

  private unwrap<T>(result: ApiResult<T>): T {
    if (result && result.success === false) {
      throw new Error(result.error || result.message || 'Request failed');
    }
    return result.data as T;
  }
}
