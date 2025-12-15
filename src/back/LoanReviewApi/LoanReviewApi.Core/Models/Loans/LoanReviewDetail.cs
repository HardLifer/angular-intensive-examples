using LoanReviewApi.Core.Models.Reviews;
using LoanReviewApi.Core.Models.Users;

namespace LoanReviewApi.Core.Models.Loans
{
    public class LoanReviewDetail
    {
        public LoanReviewDetail()
        {
            LoanReviewItems = new List<LoanReviewItem>();
        }

        public int Id { get; set; }

        public int LoanId { get; set; }

        public LoanDetail? LoanDetail { get; set; }

        public LoanDetailsComm? LoanDetailsComm { get; set; }

        public int? TemplateId { get; set; }

        public ReviewTemplate? ReviewTemplate { get; set; }

        public string? Comments { get; set; }

        public int? StatusId { get; set; }

        public LoanReviewStatus? LoanReviewStatus { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DateCompleted { get; set; }

        public int? CompletedBy { get; set; }

        public User? UserCompletedBy { get; set; }

        public int? LastUpdatedBy { get; set; }

        public User? UserUpdatedBy { get; set; }

        public ICollection<LoanReviewItem> LoanReviewItems { get; set; }
    }
}
