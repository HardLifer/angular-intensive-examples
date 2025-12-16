import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiBaseService } from './api-base.service';
import {
    LoanReviewDetailDto,
    LoanReviewStatusDto,
    LoanReviewItemDto
} from '../index';

@Injectable({
    providedIn: 'root'
})
export class LoanReviewService extends ApiBaseService {

    /**
     * Get loan review details by loan ID
     */
    getLoanReviewByLoanId(loanId: number): Observable<LoanReviewDetailDto> {
        return this.get<LoanReviewDetailDto>(`LoanReview/${loanId}`);
    }

    /**
     * Create loan review
     */
    createLoanReview(review: Partial<LoanReviewDetailDto>): Observable<LoanReviewDetailDto> {
        return this.post<LoanReviewDetailDto>('LoanReview', review);
    }

    /**
     * Update loan review
     */
    updateLoanReview(id: number, review: Partial<LoanReviewDetailDto>): Observable<LoanReviewDetailDto> {
        return this.put<LoanReviewDetailDto>(`LoanReview/${id}`, review);
    }

    /**
     * Get all review statuses
     */
    getReviewStatuses(): Observable<LoanReviewStatusDto[]> {
        return this.get<LoanReviewStatusDto[]>('LoanReview/statuses');
    }
}