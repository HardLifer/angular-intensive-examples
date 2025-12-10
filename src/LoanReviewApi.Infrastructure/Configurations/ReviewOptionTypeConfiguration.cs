using LoanReviewApi.Core.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class ReviewOptionTypeConfiguration : IEntityTypeConfiguration<ReviewOptionType>
    {
        public void Configure(EntityTypeBuilder<ReviewOptionType> builder)
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
        }
    }
}
