import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { environment } from '../../../environments/environment.dev';

@Injectable({
  providedIn: 'root'
})
export class ApiBaseService {
  protected baseUrl = environment.apiUrl;

  loading = signal(false);
  error = signal<string | null>(null);

  constructor(protected http: HttpClient) { }

  protected get<T>(url: string, options?: { headers?: HttpHeaders }): Observable<T> {
    this.loading.set(true);
    this.error.set(null);

    return this.http.get<T>(`${this.baseUrl}/${url}`, options).pipe(
      catchError(this.handleError.bind(this)),
      finalize(() => this.loading.set(false))
    );
  }

  protected post<T>(url: string, body: any, options?: { headers?: HttpHeaders }): Observable<T> {
    this.loading.set(true);
    this.error.set(null);

    return this.http.post<T>(`${this.baseUrl}/${url}`, body, options).pipe(
      catchError(this.handleError.bind(this)),
      finalize(() => this.loading.set(false))
    );
  }

  protected put<T>(url: string, body: any, options?: { headers?: HttpHeaders }): Observable<T> {
    this.loading.set(true);
    this.error.set(null);

    return this.http.put<T>(`${this.baseUrl}/${url}`, body, options).pipe(
      catchError(this.handleError.bind(this)),
      finalize(() => this.loading.set(false))
    );
  }

  protected delete<T>(url: string, options?: { headers?: HttpHeaders }): Observable<T> {
    this.loading.set(true);
    this.error.set(null);

    return this.http.delete<T>(`${this.baseUrl}/${url}`, options).pipe(
      catchError(this.handleError.bind(this)),
      finalize(() => this.loading.set(false))
    );
  }

  protected handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.status === 0) {
        errorMessage = 'Unable to connect to server. Please check if the API is running.';
      } else if (error.status === 400) {
        errorMessage = error.error?.message || 'Bad Request';
      } else if (error.status === 404) {
        errorMessage = 'Resource not found';
      } else if (error.status === 500) {
        errorMessage = 'Internal server error';
      } else {
        errorMessage = `Server Error: ${error.status} - ${error.message}`;
      }
    }

    this.error.set(errorMessage);
    console.error('API Error:', errorMessage, error);

    return throwError(() => new Error(errorMessage));
  }
}