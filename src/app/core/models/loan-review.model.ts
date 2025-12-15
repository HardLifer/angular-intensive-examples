export interface LoanReviewDetailDto {
  id: number;
  loanId: number;
  templateId: number | null;
  comments: string | null;
  statusId: number | null;
  status: string | null;
  isLocked: boolean;
  createdAt: Date | null;
  updatedAt: Date | null;
  dateCompleted: Date | null;
  completedBy: number | null;
  lastUpdatedBy: number | null;
}

export interface LoanReviewStatusDto {
  id: number;
  status: string;
}