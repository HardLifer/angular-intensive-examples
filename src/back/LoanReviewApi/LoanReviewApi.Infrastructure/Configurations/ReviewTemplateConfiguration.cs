using LoanReviewApi.Core.Models.Reviews;
using LoanReviewApi.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class ReviewTemplateConfiguration : IEntityTypeConfiguration<ReviewTemplate>
    {
        public void Configure(EntityTypeBuilder<ReviewTemplate> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.HasData(ReviewTemplatesSeeder.PopulateReviewTemplates());

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
