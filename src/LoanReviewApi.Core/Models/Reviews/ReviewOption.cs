using LoanReviewApi.Core.Models.Loans;

namespace LoanReviewApi.Core.Models.Reviews
{
    public class ReviewOption
    {
        public ReviewOption()
        {
            LoanReviewItems = new List<LoanReviewItem>();
        }

        public int Id { get; set; }

        public int OptionTypeId { get; set; }

        public ReviewOptionType ReviewOptionType { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<LoanReviewItem> LoanReviewItems { get; set; }
    }
}
