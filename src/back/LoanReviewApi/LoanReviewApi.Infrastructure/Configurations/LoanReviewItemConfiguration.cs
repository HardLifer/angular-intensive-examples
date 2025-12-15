using LoanReviewApi.Core.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class LoanReviewItemConfiguration : IEntityTypeConfiguration<LoanReviewItem>
    {
        public void Configure(EntityTypeBuilder<LoanReviewItem> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.HasIndex(ent => ent.ReviewId);

            builder.Property(ent => ent.Comment)
                .HasColumnType("nvarchar(1024)");

            builder.Property(ent => ent.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime");

            builder.Property(ent => ent.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime");

            builder.HasOne(ent => ent.ReviewOption)
                .WithMany(ent => ent.LoanReviewItems)
                .HasForeignKey(ent => ent.OptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ent => ent.TemplateItem)
                .WithMany(ent => ent.LoanReviewItems)
                .HasForeignKey(ent => ent.TemplateItemId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
