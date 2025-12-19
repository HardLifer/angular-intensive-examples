using LoanReviewApi.DTO;
using LoanReviewApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanReviewApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoanReviewDetailController : ControllerBase
	{
		// All Related Functionality to loan Review, except get -
		// it is already implemented inside of Search Controller
		private readonly ILogger<LoanReviewDetailController> _logger;
		private readonly ILoanReviewDetailService _loanReviewDetailService;

		public LoanReviewDetailController(ILogger<LoanReviewDetailController> logger, ILoanReviewDetailService loanReviewDetailService)
		{
			_logger = logger;
			_loanReviewDetailService = loanReviewDetailService;
		}

		[HttpPatch("change-loan-review-status")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> ChangeLoanReviewStatus([FromBody] ChangeStatusRequestDto changeStatusRequest, CancellationToken cancellationToken = default)
		{
			try
			{
				_logger.LogDebug("ChangeLoanReviewStatus called with LoanReviewId: {LoanReviewId} and NewStatusId: {NewStatusId}", changeStatusRequest.LoanReviewId, changeStatusRequest.NewStatusId);

				var result = await _loanReviewDetailService.ChangeLoanReviewStatus(changeStatusRequest.LoanReviewId, changeStatusRequest.NewStatusId, cancellationToken);

				if (result == false)
				{
					_logger.LogWarning("Failed to change status for LoanReviewId: {LoanReviewId}", changeStatusRequest.LoanReviewId);
					return BadRequest(new ProblemDetails
					{
						Detail = $"Failed to change status for LoanReviewId: {changeStatusRequest.LoanReviewId}. Check if the LoanReviewId and NewStatusId are correct.",
						Status = 400,
						Type = "BadRequest",
						Title = "Change Loan Review Status Failed"
					});
				}

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while changing status for LoanReviewId: {LoanReviewId}", changeStatusRequest.LoanReviewId);

				throw;
			}
		}

		[HttpDelete("delete-loan-review/{templateId:int}/{loanId:int}")]
		[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<bool>> DeleteLoanReview(int templateId, int loanId, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogDebug("DeleteLoanReview callaed with LoanReviewId: {LoanReviewId}", loanId);

				var result = await _loanReviewDetailService.DeleteLoanReview(templateId, loanId, cancellationToken);

				if (result == false)
				{
					_logger.LogWarning("Failed to delete LoanReviewId: {LoanReviewId}", loanId);
					return BadRequest(new ProblemDetails
					{
						Detail = $"Failed to delete LoanReviewId: {loanId}. Check if the LoanReviewId is correct.",
						Status = 400,
						Type = "BadRequest",
						Title = "Delete Loan Review Failed"
					});
				}

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while deleting LoanReviewId: {LoanReviewId}", loanId);

				throw;
			}
		}
	}
}
