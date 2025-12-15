using LoanReviewApi.DTO;
using LoanReviewApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ILogger<ImportController> _loger;
        private readonly IImportLoansService _importLoansService;

        public ImportController(ILogger<ImportController> loger, IImportLoansService importLoansService)
        {
            _loger = loger;
            _importLoansService = importLoansService;
        }

        [HttpPost("{templateId:int}")]
        public async Task<IActionResult> ImportExcelFile(IFormFile file, int templateId, CancellationToken cancellationToken = default)
        {
            try
            {
                //Pre-validations
                //Pre-logging
                var importFailedLoans = await _importLoansService.ImportLoans(file, templateId, cancellationToken);

                if (importFailedLoans.Any())
                {
                    return BadRequest(importFailedLoans);
                }

                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
