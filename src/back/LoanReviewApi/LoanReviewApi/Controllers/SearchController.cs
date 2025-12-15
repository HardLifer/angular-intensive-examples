using LoanReviewApi.DTO;
using LoanReviewApi.Services.DTOs;
using LoanReviewApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _searchLogger;
        private readonly ISearchLoanService _searchLoanService;

        public SearchController(ILogger<SearchController> searchLogger, ISearchLoanService searchLoanService)
        {
            _searchLogger = searchLogger;
            _searchLoanService = searchLoanService;
        }

        [HttpGet]
        public async Task<ActionResult<LoanReviewDetailDto>> SearchLoanById([FromQuery] int loanDetailId, CancellationToken cancellationToken = default)
        {
            try
            {
                var loan = await _searchLoanService.GetLoanDetailById(loanDetailId, cancellationToken);

                if (loan == null)
                {
                    return NotFound("Loan not found");
                }

                return Ok(loan);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchRequestDto searchRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _searchLoanService.Search(searchRequest, cancellationToken);

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("search-filters")]
        public async Task<ActionResult<SearchFiltersResponseDto>> GetSearchFilters([FromQuery] int templateId, CancellationToken cancellationToken = default)
        {
            try
            {
                var filters = await _searchLoanService.GetSearchFilters(templateId, cancellationToken);

                if (filters == null || !filters.Any())
                {
                    return NotFound();
                }

                var filtersDto = new SearchFiltersResponseDto((Dictionary<string, List<string>>)filters);

                return Ok(filters);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
