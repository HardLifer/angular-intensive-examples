namespace LoanReviewApi.Services.DTOs
{
    public record SearchResponseDto(List<LoanReviewDetailDto> Loans, TotalItemsDto Page);
}
