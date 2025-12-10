using LoanReviewApi.Services.DTOs;

namespace LoanReviewApi.Services.Interfaces
{
    public interface ISearchLoanService
    {
        Task<LoanDetailDto> GetLoanDetailById(int loanId, CancellationToken cancellationToken);
    }
}
