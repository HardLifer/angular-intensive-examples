namespace LoanReviewApi.Services.Interfaces
{
	public interface ILoanReviewDetailService
	{
		Task<bool> ChangeLoanReviewStatus(int loanReviewId, int newStatusId, CancellationToken cancellationToken);

		// We are making soft delete with Locked prop and Delete Time
		Task<bool> DeleteLoanReview(int templateId, int loanReviewId, CancellationToken cancellationToken);

		Task UpdateLoanReview(int loanReviewId, string comments, CancellationToken cancellationToken);
	}
}
