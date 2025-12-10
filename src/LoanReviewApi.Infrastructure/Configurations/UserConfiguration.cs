using LoanReviewApi.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.Property(ent => ent.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(ent => ent.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            builder.Property(ent => ent.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            builder.Property(ent => ent.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");

            builder.HasOne<UserRole>(ent => ent.UserRole)
                .WithMany(ent => ent.Users)
                .HasForeignKey(ent => ent.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
