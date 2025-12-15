import { Injectable } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiBaseService } from './api-base.service';
import { FailedImportLoanData } from '..';

@Injectable({
    providedIn: 'root'
})
export class ImportApiService extends ApiBaseService {

    importExcelFile(file: File, templateId: number): Observable<number | FailedImportLoanData[]> {
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
            map(event => {
                if (event.type === HttpEventType.UploadProgress) {
                    const progress = event.total
                        ? Math.round((100 * event.loaded) / event.total)
                        : 0;
                    return progress;
                } else if (event.type === HttpEventType.Response) {
                    return event.body || [];
                }
                return 0;
            })
        );
    }
}