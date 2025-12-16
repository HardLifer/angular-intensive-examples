export enum LoanReviewStatus {
  InReview = 1,
  ReviewComplete = 2,
  New = 3
}

export enum ReviewTemplateType {
  Residential = 1,
  Commercial = 2
}

export const LoanReviewStatusLabels: Record<LoanReviewStatus, string> = {
  [LoanReviewStatus.InReview]: 'In Review',
  [LoanReviewStatus.ReviewComplete]: 'Review Complete',
  [LoanReviewStatus.New]: 'New'
};

export enum ReviewTemplate {
  Residential = 1,
  Commercial = 2
}

export const ReviewTemplateLabels: Record<ReviewTemplate, string> = {
  [ReviewTemplate.Residential]: 'Residential',
  [ReviewTemplate.Commercial]: 'Commercial'
};