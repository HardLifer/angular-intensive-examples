using LoanReviewApi.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.Property(ent => ent.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.HasMany(ent => ent.Users)
                .WithOne(ent => ent.UserRole);

            builder.Property(ent => ent.RoleName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");
        }
    }
}
