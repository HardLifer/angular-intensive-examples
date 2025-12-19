using LoanReviewApi.Core.Enums;
using LoanReviewApi.Services.Interfaces;
using LoanReviewApi.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LoanReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ILogger<ImportController> _logger;
        private readonly IImportLoansService _importLoansService;

        public ImportController(ILogger<ImportController> logger, IImportLoansService importLoansService)
        {
            _logger = logger;
            _importLoansService = importLoansService;
        }

        [HttpPost("{templateId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(List<FailedImportLoanData>),(int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ImportExcelFile(IFormFile file, int templateId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("ImportExcelFile called with templateId: {TemplateId}", templateId);

                if (!Enum.TryParse<ReviewTemplates>(templateId.ToString(), true, out _))
                {
                    _logger.LogWarning("Provided template id: {TemplateId} doesn't exist in the system.", templateId);

                    return NotFound(new { Message = $"Template with id: {templateId} not found." });
                }

                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No file was provided for import.");

                    return BadRequest(new { Message = "No file provided." });
                }
                
                _logger.LogDebug("Starting import process for file: {FileName} with templateId: {TemplateId}", file.FileName, templateId);

                var importFailedLoans = await _importLoansService.ImportLoans(file, templateId, cancellationToken);

                if (importFailedLoans.Any())
                {
                    return BadRequest(importFailedLoans);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while importing the Excel file.");

                throw;
            }
        }
    }
}
