namespace LoanReviewApi.Services.DTOs
{
    public class LoanDetailDto
    {
        public int Id { get; set; }

        public int LoanId { get; set; }

        public float? Balance { get; set; }

        public float? LTV { get; set; }

        public string? InsuranceType { get; set; }
    }
}
