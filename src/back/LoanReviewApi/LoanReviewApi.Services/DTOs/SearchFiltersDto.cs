namespace LoanReviewApi.Services.DTOs
{
    public record SearchFiltersDto(IDictionary<string, List<string>> ActiveFilters);
}
