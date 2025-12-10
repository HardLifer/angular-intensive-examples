using LoanReviewApi.Core.Models.Loans;

namespace LoanReviewApi.Core.Models.Users
{
    public class User
    {
        public User()
        {
            UsersCreatedReviewDetails = new List<LoanReviewDetail>();
            UsersUpdatedReviewDetails = new List<LoanReviewDetail>();
        }

        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }

        public UserRole UserRole { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Title { get; set; }

        public ICollection<LoanReviewDetail> UsersCreatedReviewDetails { get; set; }

        public ICollection<LoanReviewDetail> UsersUpdatedReviewDetails { get; set; }
    }
}
