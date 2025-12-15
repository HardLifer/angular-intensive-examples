using LoanReviewApi.Core.Enums;

namespace LoanReviewApi.Services.DTOs
{
    public class LoanReviewDetailDto
    {
        public int Id { get; set; }

        public int LoanId { get; set; }

        public string? BorrowerName { get; set; }  

        public DateTime? Date { get; set; }

        public LoanReviewStatuses Status { get; set; }

        public string? Director { get; set; }

        public string? Branch { get; set; }

        public string? ProductCode { get; set; }

        public string? ReviewedBy { get; set; }

        public string? MortgageOfficer { get; set; }

        public string? Province { get; set; }

        public string? ReviewType { get; set; }
    }
}
