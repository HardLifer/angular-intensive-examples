namespace LoanReviewApi.Core.Models.Loans
{
    public class LoanReviewStatus
    {
        public LoanReviewStatus()
        {
            LoanReviewDetails = new List<LoanReviewDetail>();
        }

        public int Id { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<LoanReviewDetail> LoanReviewDetails { get; set; }

    }
}
