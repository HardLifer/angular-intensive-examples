using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommTableAndChangeFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanReviewDetails_LoanReviews_LoanDetailId",
                table: "LoanReviewDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanReviews",
                table: "LoanReviews");

            migrationBuilder.RenameTable(
                name: "LoanReviews",
                newName: "LoanDetails");

            migrationBuilder.RenameIndex(
                name: "IX_LoanReviews_LoanId",
                table: "LoanDetails",
                newName: "IX_LoanDetails_LoanId");

            migrationBuilder.AddColumn<int>(
                name: "LoanDetailsCommId",
                table: "LoanReviewDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanDetails",
                table: "LoanDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LoanDetailsComm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    SSMA_TimeStamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: false),
                    Obs = table.Column<float>(type: "real", nullable: true),
                    ProductBusinessGroupCode = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    LoanClass = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    InsuranceType = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    VariableRateFlag = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    LTV = table.Column<float>(type: "real", nullable: true),
                    Balance = table.Column<float>(type: "real", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Beacon = table.Column<float>(type: "real", nullable: true),
                    RiskScore = table.Column<float>(type: "real", nullable: true),
                    AmortizationTermInMonths = table.Column<float>(type: "real", nullable: true),
                    TermInMonths = table.Column<float>(type: "real", nullable: true),
                    SecurityTypeCode = table.Column<float>(type: "real", nullable: true),
                    SecurityTypeDescription = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    CommitmentDate = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    MaturityDate = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ApplicantCifNumber = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    BrokerCompany = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    EffectiveRate = table.Column<float>(type: "real", nullable: true),
                    ExceptionFlag = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ExceptionType = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    OriginalTdsRatio = table.Column<float>(type: "real", nullable: true),
                    LoanConformingIndicator = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ReasonCode = table.Column<float>(type: "real", nullable: true),
                    ScoringValue = table.Column<float>(type: "real", nullable: true),
                    EmploymentType = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Income = table.Column<float>(type: "real", nullable: true),
                    ProductText = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    FundingDate = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PriorEncCount = table.Column<float>(type: "real", nullable: true),
                    PriorEncAmount = table.Column<float>(type: "real", nullable: true),
                    BranchOfficeId = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    BranchOfficeText = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    BorrowerName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    UnderwriterSkey = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PartnerName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PartnerId = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Director = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Date1 = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    ReviewType = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    NonConformingReason = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    MortgageOfficerBp = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    MortgageOfficerName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    MortgageOfficerSkey = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    AggRating = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PrRating = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    SpRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    StressTDS = table.Column<float>(type: "real", nullable: true),
                    isLocked = table.Column<byte>(type: "tinyint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanDetailsComm", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewDetails_LoanDetailsCommId",
                table: "LoanReviewDetails",
                column: "LoanDetailsCommId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDetailsComm_LoanId",
                table: "LoanDetailsComm",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanReviewDetails_LoanDetailsComm_LoanDetailsCommId",
                table: "LoanReviewDetails",
                column: "LoanDetailsCommId",
                principalTable: "LoanDetailsComm",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanReviewDetails_LoanDetails_LoanDetailId",
                table: "LoanReviewDetails",
                column: "LoanDetailId",
                principalTable: "LoanDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanReviewDetails_LoanDetailsComm_LoanDetailsCommId",
                table: "LoanReviewDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanReviewDetails_LoanDetails_LoanDetailId",
                table: "LoanReviewDetails");

            migrationBuilder.DropTable(
                name: "LoanDetailsComm");

            migrationBuilder.DropIndex(
                name: "IX_LoanReviewDetails_LoanDetailsCommId",
                table: "LoanReviewDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanDetails",
                table: "LoanDetails");

            migrationBuilder.DropColumn(
                name: "LoanDetailsCommId",
                table: "LoanReviewDetails");

            migrationBuilder.RenameTable(
                name: "LoanDetails",
                newName: "LoanReviews");

            migrationBuilder.RenameIndex(
                name: "IX_LoanDetails_LoanId",
                table: "LoanReviews",
                newName: "IX_LoanReviews_LoanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanReviews",
                table: "LoanReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanReviewDetails_LoanReviews_LoanDetailId",
                table: "LoanReviewDetails",
                column: "LoanDetailId",
                principalTable: "LoanReviews",
                principalColumn: "Id");
        }
    }
}
