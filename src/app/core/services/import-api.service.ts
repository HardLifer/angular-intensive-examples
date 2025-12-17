import { Injectable } from '@angular/core';
import { HttpClient, HttpEventType, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiBaseService } from './api-base.service';
import { FailedImportLoanData } from '../index';

export interface ImportProgress {
    type: 'progress' | 'complete' | 'error';
    progress?: number;
    data?: FailedImportLoanData[];
    error?: string;
}

@Injectable({
    providedIn: 'root'
})
export class ImportApiService extends ApiBaseService {

    /**
     * Import Excel file
     * Maps to: POST /api/Import/{templateId}
     */
    importExcelFile(file: File, templateId: number): Observable<ImportProgress> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post<FailedImportLoanData[]>(
            `${this.baseUrl}/Import/${templateId}`,
            formData,
            {
                reportProgress: true,
                observe: 'events'
            }
        ).pipe(
            map(event => this.handleImportEvent(event)),
            catchError(error => {
                this.handleError(error);
                return [{
                    type: 'error' as const,
                    error: error.message
                }];
            })
        );
    }

    private handleImportEvent(event: HttpEvent<FailedImportLoanData[]>): ImportProgress {
        if (event.type === HttpEventType.UploadProgress) {
            const progress = event.total
                ? Math.round((100 * event.loaded) / event.total)
                : 0;
            return {
                type: 'progress',
                progress
            };
        } else if (event.type === HttpEventType.Response) {
            const failedLoans = event.body || [];
            return {
                type: 'complete',
                data: failedLoans,
                progress: 100
            };
        }
        return { type: 'progress', progress: 0 };
    }
}