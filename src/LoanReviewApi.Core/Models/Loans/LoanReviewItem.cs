using LoanReviewApi.Core.Models.Reviews;

namespace LoanReviewApi.Core.Models.Loans
{
    public class LoanReviewItem
    {
        public int Id { get; set; }

        public int ReviewId { get; set; }

        public LoanReviewDetail LoanReviewDetail { get; set; } = null!;

        public int? TemplateItemId { get; set; }

        public ReviewTemplateItem? TemplateItem { get; set; }

        public int? OptionId { get; set; }

        public ReviewOption? ReviewOption { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
