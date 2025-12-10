using LoanReviewApi.Core.Models.Loans;
using LoanReviewApi.Core.Models.Reviews;
using LoanReviewApi.Core.Models.Users;
using LoanReviewApi.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LoanReviewApi.Infrastructure.Context
{
    public class LoanReviewContext : DbContext
    {
        public LoanReviewContext()
        {            
        }

        public LoanReviewContext(DbContextOptions<LoanReviewContext> dbOptions)
            : base(options: dbOptions)
        {
        }

        public DbSet<LoanDetail> LoanDetails { get; set; }

        public DbSet<LoanDetailsComm> LoanDetailsComm { get; set; }

        public DbSet<LoanReviewDetail> LoanReviewDetails { get; set; }

        public DbSet<LoanReviewItem> LoanReviewItems { get; set; }

        public DbSet<LoanReviewReports> LoanReviewReports { get; set; }

        public DbSet<LoanReviewStatus> LoanReviewStatuses { get; set; }

        public DbSet<ReviewOption> ReviewOptions { get; set; }

        public DbSet<ReviewOptionType> ReviewOptionTypes { get; set; }

        public DbSet<ReviewTemplate> ReviewTemplates { get; set; }

        public DbSet<ReviewTemplateItem> ReviewTemplateItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanDetailConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
