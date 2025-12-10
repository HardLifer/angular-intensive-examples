using LoanReviewApi.Core.Models.Loans;
using LoanReviewApi.Infrastructure.Context;
using LoanReviewApi.Services.DTOs;
using LoanReviewApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoanReviewApi.Services.Search
{
    public class SearchLoanService : ISearchLoanService
    {
        private readonly ILogger<SearchLoanService> _logger;
        private readonly IDbContextFactory<LoanReviewContext> _dbContextFactory;

        public SearchLoanService(ILogger<SearchLoanService> logger, IDbContextFactory<LoanReviewContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<LoanDetailDto> GetLoanDetailById(int loanId, CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var loanDetailDto = await dbContext.LoanDetails
                .Select(ent => new LoanDetailDto
                {
                    Id = ent.Id,
                    LoanId = ent.LoanId,
                    Balance = ent.Balance,
                    LTV = ent.LTV,
                    InsuranceType = ent.InsuranceType,
                })
                .FirstOrDefaultAsync(ent => ent.LoanId == loanId);

            if (loanDetailDto == null)
            {
                // Log Error
            }

            return loanDetailDto!;
        }
    }
}
