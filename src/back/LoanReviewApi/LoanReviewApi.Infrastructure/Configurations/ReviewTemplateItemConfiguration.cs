using LoanReviewApi.Core.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class ReviewTemplateItemConfiguration : IEntityTypeConfiguration<ReviewTemplateItem>
    {
        public void Configure(EntityTypeBuilder<ReviewTemplateItem> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.Property(ent => ent.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(ent => ent.HasComment)
                .HasColumnName("hasComment");

            builder.Property(ent => ent.IsDisabled)
                .HasColumnName("isDisabled");

            builder.Property(ent => ent.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime");

            builder.Property(ent => ent.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime");

            builder.HasOne(ent => ent.OptionType)
                .WithMany(ent => ent.ReviewTemplateItems)
                .HasForeignKey(ent => ent.ItemOptionTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ent => ent.ReviewTemplate)
                .WithMany(ent => ent.ReviewTemplateItems)
                .HasForeignKey(ent => ent.ItemOptionTypeId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
