using LoanReviewApi.Core.Models.Loans;

namespace LoanReviewApi.Core.Models.Reviews
{
    public class ReviewTemplateItem
    {
        public ReviewTemplateItem()
        {
            LoanReviewItems = new List<LoanReviewItem>();
        }

        public int Id { get; set; }

        public int? TemplateId { get; set; }

        public ReviewTemplate? ReviewTemplate { get; set; }

        public int? ItemOptionTypeId { get; set; }

        public ReviewOptionType? OptionType { get; set; }

        public bool HasComment { get; set; }

        public bool IsDisabled { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<LoanReviewItem> LoanReviewItems { get; set; }
    }
}
