export enum LoanReviewStatus {
  InReview = 1,
  ReviewComplete = 2,
  New = 3
}

export const LoanReviewStatusLabels: Record<LoanReviewStatus, string> = {
  [LoanReviewStatus.InReview]: 'In Review',
  [LoanReviewStatus.ReviewComplete]: 'Review Complete',
  [LoanReviewStatus.New]: 'New'
};