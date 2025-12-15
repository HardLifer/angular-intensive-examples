using LoanReviewApi.Core.Models.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanReviewApi.Infrastructure.Configurations
{
    internal class LoanDetailsCommConfiguration : IEntityTypeConfiguration<LoanDetailsComm>
    {
        public void Configure(EntityTypeBuilder<LoanDetailsComm> builder)
        {
            builder.HasKey(ent => ent.Id);

            builder.HasIndex(ent => ent.LoanId);

            builder.Property(ent => ent.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(ent => ent.LoanId)
                .IsRequired();

            builder.Property(ent => ent.ProductBusinessGroupCode)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.LoanClass)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.InsuranceType)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.VariableRateFlag)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.Province)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.Purpose)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.SecurityTypeDescription)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.CommitmentDate)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.MaturityDate)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.ApplicantCifNumber)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.BrokerCompany)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.ExceptionFlag)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.ExceptionType)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.LoanConformingIndicator)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.EmploymentType)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.Occupation)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.ProductText)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.FundingDate)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.BranchOfficeId)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.BranchOfficeText)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.BorrowerName)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.UnderwriterSkey)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.PartnerName)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.PartnerId)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.Director)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.Date1)
                .HasColumnType("datetime2(0)");

            builder.Property(ent => ent.ReviewType)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.NonConformingReason)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.MortgageOfficerBp)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.MortgageOfficerName)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.MortgageOfficerSkey)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.AggRating)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.PrRating)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.UserName)
                .HasColumnType("nvarchar(255)");

            builder.Property(ent => ent.IsLocked)
                .HasColumnType("tinyint")
                .HasColumnName("isLocked");

            builder.Property(ent => ent.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            builder.Property(ent => ent.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            builder.Property(ent => ent.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");

            builder.Property(ent => ent.SSMA_TimeStamp)
                .HasColumnType("timestamp")
                .HasColumnName("SSMA_TimeStamp");
        }
    }
}
