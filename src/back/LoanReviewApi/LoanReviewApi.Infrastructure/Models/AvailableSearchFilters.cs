using LoanReviewApi.Core.Enums;

namespace LoanReviewApi.Infrastructure.Models
{
    public class AvailableSearchFilters
    {
        public int? LoanId { get; init; }

        public DateTime? Month { get; init; }

        public string? ProductCode { get; init; }

        public LoanReviewStatuses? Status { get; init; }

        public string? ReviewType { get; init; }

        public string? Team { get; init; }

        public string? Director { get; init; }

        public string? Reviewer { get; init; }

        public DateTime? QuarterStart { get; init; }

        public DateTime? QuarterEnd { get; init; }
    }
}
