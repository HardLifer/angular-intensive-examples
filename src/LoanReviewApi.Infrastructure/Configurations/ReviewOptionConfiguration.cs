using LoanReviewApi.Core.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class ReviewOptionConfiguration : IEntityTypeConfiguration<ReviewOption>
    {
        public void Configure(EntityTypeBuilder<ReviewOption> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.Property(ent => ent.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(ent => ent.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime");

            builder.Property(ent => ent.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime");

            builder.HasOne(ent => ent.ReviewOptionType)
                .WithMany(ent => ent.ReviewOptions)
                .HasForeignKey(ent => ent.OptionTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
