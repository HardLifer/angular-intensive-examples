import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiBaseService } from './api-base.service';
import { LoanDetailDto, GetLoanByIdRequest } from '..';

@Injectable({
    providedIn: 'root'
})
export class LoanApiService extends ApiBaseService {

  /**
   * Get loan detail by ID
   * Maps to: GET /api/LoanDetails?loanDetailId={id}
   */
  getLoanById(loanDetailId: number): Observable<LoanDetailDto> {
    return this.get<LoanDetailDto>(`LoanDetails?loanDetailId=${loanDetailId}`);
  }

  /**
   * Search loans (when backend endpoint is implemented)
   * Future endpoint: GET /api/LoanDetails/search
   */
  searchLoans(searchText: string, pageIndex: number, pageSize: number): Observable<any> {
    const params = `search?text=${encodeURIComponent(searchText)}&page=${pageIndex}&size=${pageSize}`;
    return this.get(`LoanDetails/${params}`);
  }

  /**
   * Get all loans with pagination (when backend endpoint is implemented)
   * Future endpoint: GET /api/LoanDetails/list
   */
  getAllLoans(pageIndex: number, pageSize: number): Observable<any> {
    return this.get(`LoanDetails/list?page=${pageIndex}&size=${pageSize}`);
  }
}