using LoanReviewApi.Core.Models.Loans;
using LoanReviewApi.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class LoanReviewStatusConfiguration : IEntityTypeConfiguration<LoanReviewStatus>
    {
        public void Configure(EntityTypeBuilder<LoanReviewStatus> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.HasData(LoanReviewStatusSeeder.PopulateStatusData());

            builder.Property(ent => ent.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(ent => ent.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime");

            builder.Property(ent => ent.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime");
        }
    }
}
