export interface LoanReviewDetailDto {
  id: number;
  loanId: number;
  templateId: number | null;
  comments: string | null;
  statusId: number | null;
  isLocked: boolean;
  createdAt: Date | null;
  updatedAt: Date | null;
  dateCompleted: Date | null;
  completedBy: number | null;
  lastUpdatedBy: number | null;
}

export interface LoanReviewItemDto {
  id: number;
  reviewId: number;
  templateItemId: number | null;
  optionId: number | null;
  comment: string | null;
  createdAt: Date | null;
  updatedAt: Date;
}

export interface LoanReviewStatusDto {
  id: number;
  status: string;
  createdAt: Date;
  updatedAt: Date | null;
}

export interface LoanReviewReportsDto {
  id: number;
  l1L2Count: number | null;
  l1Count: number | null;
  l2Count: number | null;
  l2TdsIncCount: number | null;
  l2OtherCount: number | null;
  cdCount: number | null;
  refCompCount: number | null;
  decisionCount: number | null;
}