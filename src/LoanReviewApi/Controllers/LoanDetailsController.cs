using LoanReviewApi.Services.DTOs;
using LoanReviewApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanDetailsController : ControllerBase
    {
        private readonly ILogger<LoanDetailsController> _logger;
        private readonly ISearchLoanService _searchLoanService;

        public LoanDetailsController(ILogger<LoanDetailsController> logger, ISearchLoanService searchLoanService)
        {
            _logger = logger;
            _searchLoanService = searchLoanService;
        }

        [HttpGet]
        public async Task<ActionResult<LoanDetailDto>> GetLoanById([FromQuery] int loanDetailId, CancellationToken cancellationToken = default)
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
    }
}
