export interface LoanSearchRequest {
  searchText?: string;
  pageIndex: number;
  pageSize: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}

export interface LoanSearchResponse<T> {
  data: T[];
  totalCount: number;
  pageIndex: number;
  pageSize: number;
}

export interface GetLoanByIdRequest {
  loanDetailId: number;
}