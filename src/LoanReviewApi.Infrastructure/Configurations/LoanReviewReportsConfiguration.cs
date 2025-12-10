using LoanReviewApi.Core.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class LoanReviewReportsConfiguration : IEntityTypeConfiguration<LoanReviewReports>
    {
        public void Configure(EntityTypeBuilder<LoanReviewReports> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.Property(ent => ent.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
