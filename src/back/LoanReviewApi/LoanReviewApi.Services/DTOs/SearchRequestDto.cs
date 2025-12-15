using LoanReviewApi.Infrastructure.Models;
using LoanReviewApi.Services.Models;

namespace LoanReviewApi.Services.DTOs
{
    public record SearchRequestDto(int TemplateId, Pagination Pagination, AvailableSearchFilters Filters);
}
