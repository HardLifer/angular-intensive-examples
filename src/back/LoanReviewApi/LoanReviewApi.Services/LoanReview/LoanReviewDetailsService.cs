using LoanReviewApi.Core.Enums;
using LoanReviewApi.Infrastructure.Context;
using LoanReviewApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoanReviewApi.Services.LoanReview
{
	public class LoanReviewDetailsService : ILoanReviewDetailService
	{
		private readonly ILogger<LoanReviewDetailsService> _logger;
		private readonly IDbContextFactory<LoanReviewContext> _dbContextFactory;

		public LoanReviewDetailsService(ILogger<LoanReviewDetailsService> logger, IDbContextFactory<LoanReviewContext> dbContextFactory)
		{
			_logger = logger;
			_dbContextFactory = dbContextFactory;
		}

		public async Task<bool> ChangeLoanReviewStatus(int loanReviewId, int newStatusId, CancellationToken cancellationToken)
		{
			var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

			var loanReview = await dbContext.LoanReviewDetails
				.FirstOrDefaultAsync(lr => lr.Id == loanReviewId, cancellationToken);

			if (loanReview == null)
			{
				_logger.LogWarning("LoanReview with Id {LoanReviewId} not found.", loanReviewId);

				return false;
			}

			if (!Enum.TryParse<LoanReviewStatuses>(newStatusId.ToString(), true, out _))
			{
				_logger.LogError("Provided status id = {StatusId} doesn't exist. Check it out or contact administrator to add a new one", newStatusId);

				return false;
			}

			loanReview.StatusId = newStatusId;
			await dbContext.SaveChangesAsync(cancellationToken);

			return true;
		}

		public async Task<bool> DeleteLoanReview(int templateId, int loanReviewId, CancellationToken cancellationToken)
		{
			var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

			var loanReview = await dbContext.LoanReviewDetails
				.Include(ent => ent.LoanDetail)
				.Include(ent => ent.LoanDetailsComm)
				.Where(ent => ent.TemplateId == templateId) 
				.FirstOrDefaultAsync(lr => lr.Id == loanReviewId, cancellationToken);

			if (loanReview == null)
			{
				_logger.LogWarning("LoanReview with Id {LoanReviewId} not found.", loanReviewId);

				return false;
			}

			if (!Enum.TryParse<ReviewTemplates>(templateId.ToString(), true, out _))
			{
				_logger.LogError("Provided template id = {TemplateId} doesn't exist. Check it out or contact administrator to add a new one", templateId);

				return false;
			}

			loanReview.IsLocked = true;

			if (loanReview.LoanDetail != null)
			{
				loanReview.LoanDetail.IsLocked = 1;
				loanReview.LoanDetail.DeletedAt = DateTime.UtcNow;
			}

			if (loanReview.LoanDetailsComm != null)
			{
				loanReview.LoanDetailsComm.IsLocked = 1;
				loanReview.LoanDetailsComm.DeletedAt = DateTime.UtcNow;
			}

			await dbContext.SaveChangesAsync(cancellationToken);

			return true;
		}

		public Task UpdateLoanReview(int loanReviewId, string comments, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
