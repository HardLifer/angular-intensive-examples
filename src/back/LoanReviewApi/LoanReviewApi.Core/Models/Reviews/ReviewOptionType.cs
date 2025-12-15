namespace LoanReviewApi.Core.Models.Reviews
{
    public class ReviewOptionType
    {
        public ReviewOptionType()
        {
            ReviewOptions = new List<ReviewOption>();
            ReviewTemplateItems = new List<ReviewTemplateItem>();
        }

        public int Id { get; set; }

        public string OptionName { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<ReviewOption> ReviewOptions { get; set; }

        public ICollection<ReviewTemplateItem> ReviewTemplateItems { get; set; }
    }
}
