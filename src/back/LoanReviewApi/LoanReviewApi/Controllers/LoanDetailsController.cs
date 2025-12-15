using LoanReviewApi.DTO;
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

        
    }
}
