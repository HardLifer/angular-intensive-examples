using FluentValidation;
using LoanReviewApi.DTO;
using LoanReviewApi.Services.DTOs;
using LoanReviewApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LoanReviewApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private readonly ILogger<SearchController> _searchLogger;
		private readonly ISearchLoanService _searchLoanService;
		private readonly IValidator<SearchRequestDto> _searchRequestValidator;

		public SearchController(ILogger<SearchController> searchLogger, ISearchLoanService searchLoanService, IValidator<SearchRequestDto> searchRequestValidator)
		{
			_searchLogger = searchLogger;
			_searchLoanService = searchLoanService;
			_searchRequestValidator = searchRequestValidator;
		}

		[HttpGet("search-loan-by-id/{templateId:int}/{loanId:int}")]
		[ProducesResponseType(typeof(LoanReviewDetailDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<LoanReviewDetailDto>> SearchLoanById(int templateId, int loanId, CancellationToken cancellationToken = default)
		{
			try
			{
				_searchLogger.LogDebug("SearchLoanById called with TemplateId: {TemplateId} and LoanId: {LoanId}", templateId, loanId);

				var loan = await _searchLoanService.GetLoanDetailById(loanId, templateId, cancellationToken);

				if (loan == null)
				{
					_searchLogger.LogInformation("Loan not found for TemplateId: {TemplateId} and LoanId: {LoanId}", templateId, loanId);

					return NotFound(new ProblemDetails
					{
						Detail = $"Loan not found for TemplateId: {templateId} and LoanId: {loanId}",
						Status = (int)HttpStatusCode.NotFound,
						Type = "NotFound",
						Title = "Loan Not Found"
					});
				}

				return Ok(loan);
			}
			catch (Exception ex)
			{
				_searchLogger.LogError(ex, "An error occurred while searching for LoanId: {LoanId} with TemplateId: {TemplateId}", loanId, templateId);

				throw;
			}
		}

		[HttpPost("search")]
		[ProducesResponseType(typeof(SearchResponseDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<ActionResult<SearchResponseDto>> Search(SearchRequestDto searchRequest, CancellationToken cancellationToken = default)
		{
			try
			{
				_searchLogger.LogDebug("Search called with TemplateId: {TemplateId}", searchRequest.TemplateId);

				await _searchRequestValidator.ValidateAndThrowAsync(searchRequest, cancellationToken);

				var result = await _searchLoanService.Search(searchRequest, cancellationToken);

				return Ok(result);
			}
			catch (Exception ex)
			{
				_searchLogger.LogError(ex, "An error occurred while processing the search request for TemplateId: {TemplateId}", searchRequest.TemplateId);

				throw;
			}
		}

		[HttpGet("search-filters")]
		[ProducesResponseType(typeof(SearchFiltersResponseDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<SearchFiltersResponseDto>> GetSearchFilters([FromQuery] int templateId, CancellationToken cancellationToken = default)
		{
			try
			{
				_searchLogger.LogDebug("GetSearchFilters called with templateId: {TemplateId}", templateId);

				var filters = await _searchLoanService.GetSearchFilters(templateId, cancellationToken);

				if (filters == null || !filters.Any())
				{
					return NotFound(new ProblemDetails
					{
						Detail = $"No filters found for templateId: {templateId}",
						Status = (int)HttpStatusCode.NotFound,
						Type = "NotFound",
						Title = "Filters Not Found"
					});
				}

				var filtersDto = new SearchFiltersResponseDto((Dictionary<string, List<string>>)filters);

				_searchLogger.LogDebug("Retrieved {FilterCount} filters for templateId: {TemplateId}", filters.Count, templateId);

				return Ok(filters);
			}
			catch (Exception ex)
			{
				_searchLogger.LogError(ex, "An error occurred while retrieving search filters for templateId: {TemplateId}", templateId);

				throw;
			}
		}
	}
}
