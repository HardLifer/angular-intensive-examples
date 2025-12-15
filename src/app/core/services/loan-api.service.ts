import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiBaseService } from './api-base.service';
import { LoanDetailDto, GetLoanByIdRequest } from '..';

@Injectable({
    providedIn: 'root'
})
export class LoanApiService extends ApiBaseService {

    getLoanById(loanDetailId: number): Observable<LoanDetailDto> {
        return this.get<LoanDetailDto>(`LoanDetails?loanDetailId=${loanDetailId}`);
    }

    // Future: Add pagination, search, filters
    searchLoans(searchText: string, pageIndex: number, pageSize: number): Observable<any> {
        // This endpoint doesn't exist in your backend yet, but structure for future
        return this.get(`LoanDetails/search?text=${searchText}&page=${pageIndex}&size=${pageSize}`);
    }
}