using LoanReviewApi.Core.Models.Reviews;

namespace LoanReviewApi.Infrastructure.Seeders
{
    internal static class ReviewTemplatesSeeder
    {
        internal static IEnumerable<ReviewTemplate> PopulateReviewTemplates()
        {
            return new ReviewTemplate[]
            {
                new ReviewTemplate()
                {
                    Id = 1,
                    Name = "Residential",
                    CreatedAt = new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified)
                },
                new ReviewTemplate()
                {
                    Id = 2,
                    Name = "Commercial",
                    CreatedAt = new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified)
                }
            };
        }
    }
}
