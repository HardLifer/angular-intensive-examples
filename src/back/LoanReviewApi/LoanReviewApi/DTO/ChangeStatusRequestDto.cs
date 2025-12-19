namespace LoanReviewApi.DTO
{
	public record ChangeStatusRequestDto (int LoanReviewId, int NewStatusId);
}
