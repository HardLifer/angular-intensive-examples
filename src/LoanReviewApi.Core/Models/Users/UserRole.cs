namespace LoanReviewApi.Core.Models.Users
{
    public class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        public string RoleName { get; set; } = null!;

        public ICollection<User> Users { get; set; }
    }
}
