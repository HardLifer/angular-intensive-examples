using LoanReviewApi.Core.Models.Loans;

namespace LoanReviewApi.Infrastructure.Seeders
{
    internal static class LoanReviewStatusSeeder
    {
        internal static IEnumerable<LoanReviewStatus> PopulateStatusData()
        {
            return new LoanReviewStatus[]
            {
                new LoanReviewStatus
                {
                    Id = 1,
                    Status = "In Review",
                    CreatedAt = new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified)
                },
                new LoanReviewStatus
                {
                    Id = 2,
                    Status = "Review Complete",
                    CreatedAt = new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified)
                },
                new LoanReviewStatus
                {
                    Id = 3,
                    Status = "New",
                    CreatedAt = new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified)
                }
            };
        }
    }
}
