using LoanReviewApi.Services.DTOs;

namespace LoanReviewApi.Services.Interfaces
{
    public interface ISearchLoanService
    {
        Task<LoanReviewDetailDto> GetLoanDetailById(int loanId, int templateId, CancellationToken cancellationToken);

        Task<IDictionary<string, List<string>>> GetSearchFilters(int templateId, CancellationToken cancellationToken);

        Task<SearchResponseDto> Search(SearchRequestDto searchRequest, CancellationToken cancellationToken);
    }
}
