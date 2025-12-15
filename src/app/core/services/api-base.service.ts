import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, finalize } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiBaseService {
  protected readonly baseUrl = 'http://localhost:5081/api';
  
  loading = signal(false);
  error = signal<string | null>(null);

  constructor(protected http: HttpClient) {}

  protected get<T>(url: string): Observable<T> {
    this.loading.set(true);
    this.error.set(null);
    
    return this.http.get<T>(`${this.baseUrl}/${url}`).pipe(
      catchError(this.handleError.bind(this)),
      finalize(() => this.loading.set(false))
    );
  }

  protected post<T>(url: string, body: any): Observable<T> {
    this.loading.set(true);
    this.error.set(null);
    
    return this.http.post<T>(`${this.baseUrl}/${url}`, body).pipe(
      catchError(this.handleError.bind(this)),
      finalize(() => this.loading.set(false))
    );
  }

  protected handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      errorMessage = `Server Error: ${error.status} - ${error.message}`;
    }
    
    this.error.set(errorMessage);
    console.error(errorMessage);
    
    return throwError(() => new Error(errorMessage));
  }
}