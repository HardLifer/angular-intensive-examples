namespace LoanReviewApi.Services.Models
{
    public record Pagination(int PageNumber, int PageSize, int TotalItems, int TotalPages);
}
