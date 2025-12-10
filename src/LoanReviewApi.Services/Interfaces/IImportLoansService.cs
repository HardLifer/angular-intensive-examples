using LoanReviewApi.Services.Models;
using Microsoft.AspNetCore.Http;

namespace LoanReviewApi.Services.Interfaces
{
    public interface IImportLoansService
    {
        Task<IEnumerable<FailedImportLoanData>> ImportLoans(IFormFile file, int templateId, CancellationToken cancellationToken = default);
    }
}
