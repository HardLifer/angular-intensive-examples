using LoanReviewApi.Core.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class LoanReviewDetailConfiguration : IEntityTypeConfiguration<LoanReviewDetail>
    {
        public void Configure(EntityTypeBuilder<LoanReviewDetail> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.HasIndex(ent => ent.LoanId);

            builder.Property(ent => ent.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(ent => ent.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime");

            builder.Property(ent => ent.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime");

            builder.Property(ent => ent.IsLocked)
                .HasColumnName("isLocked");

            builder.Property(ent => ent.Comments)
                .HasColumnType("nvarchar(1024)");

            builder.Property(ent => ent.LoanId)
                .IsRequired();

            builder.HasOne(ent => ent.LoanDetail)
                .WithMany(ent => ent.LoanReviewDetails)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ent => ent.LoanDetailsComm)
                .WithMany(ent => ent.LoanReviewDetails)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ent => ent.UserCompletedBy)
                .WithMany(ent => ent.UsersCreatedReviewDetails)
                .HasForeignKey(ent => ent.CompletedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ent => ent.UserUpdatedBy)
                .WithMany(ent => ent.UsersUpdatedReviewDetails)
                .HasForeignKey(ent => ent.LastUpdatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ent => ent.LoanReviewStatus)
                .WithMany(ent => ent.LoanReviewDetails)
                .HasForeignKey(ent => ent.StatusId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ent => ent.LoanReviewItems)
                .WithOne(ent => ent.LoanReviewDetail)
                .HasForeignKey(ent => ent.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ent => ent.ReviewTemplate)
                .WithMany(ent => ent.LoanReviewDetails)
                .HasForeignKey(ent => ent.TemplateId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
