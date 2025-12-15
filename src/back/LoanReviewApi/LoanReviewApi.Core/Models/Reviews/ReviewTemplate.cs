using LoanReviewApi.Core.Models.Loans;

namespace LoanReviewApi.Core.Models.Reviews
{
    public class ReviewTemplate
    {
        public ReviewTemplate()
        {
            LoanReviewDetails = new List<LoanReviewDetail>();
            ReviewTemplateItems = new List<ReviewTemplateItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<LoanReviewDetail> LoanReviewDetails { get; set; }

        public ICollection<ReviewTemplateItem> ReviewTemplateItems { get; set; }
    }
}
