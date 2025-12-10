using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanReviewReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    L1L2Count = table.Column<int>(type: "int", nullable: true),
                    L1Count = table.Column<int>(type: "int", nullable: true),
                    L2Count = table.Column<int>(type: "int", nullable: true),
                    L2TDSIncCount = table.Column<int>(type: "int", nullable: true),
                    L2OtherCount = table.Column<int>(type: "int", nullable: true),
                    CDCount = table.Column<int>(type: "int", nullable: true),
                    RefCompCount = table.Column<int>(type: "int", nullable: true),
                    DecisionCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanReviewReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanReviews",
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
                    EffetiveRate = table.Column<float>(type: "real", nullable: true),
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
                    Date1 = table.Column<string>(type: "nvarchar(255)", nullable: true),
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
                    table.PrimaryKey("PK_LoanReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanReviewStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanReviewStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewOptionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewOptionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewOptions_ReviewOptionTypes_OptionTypeId",
                        column: x => x.OptionTypeId,
                        principalTable: "ReviewOptionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewTemplateItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    ItemOptionTypeId = table.Column<int>(type: "int", nullable: true),
                    hasComment = table.Column<bool>(type: "bit", nullable: false),
                    isDisabled = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewTemplateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewTemplateItems_ReviewOptionTypes_ItemOptionTypeId",
                        column: x => x.ItemOptionTypeId,
                        principalTable: "ReviewOptionTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReviewTemplateItems_ReviewTemplates_ItemOptionTypeId",
                        column: x => x.ItemOptionTypeId,
                        principalTable: "ReviewTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoanReviewDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    LoanDetailId = table.Column<int>(type: "int", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanReviewDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanReviewDetails_LoanReviewStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "LoanReviewStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanReviewDetails_LoanReviews_LoanDetailId",
                        column: x => x.LoanDetailId,
                        principalTable: "LoanReviews",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanReviewDetails_ReviewTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "ReviewTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanReviewDetails_Users_CompletedBy",
                        column: x => x.CompletedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanReviewDetails_Users_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoanReviewItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    TemplateItemId = table.Column<int>(type: "int", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanReviewItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanReviewItems_LoanReviewDetails_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "LoanReviewDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanReviewItems_ReviewOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "ReviewOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LoanReviewItems_ReviewTemplateItems_TemplateItemId",
                        column: x => x.TemplateItemId,
                        principalTable: "ReviewTemplateItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewDetails_CompletedBy",
                table: "LoanReviewDetails",
                column: "CompletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewDetails_LastUpdatedBy",
                table: "LoanReviewDetails",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewDetails_LoanDetailId",
                table: "LoanReviewDetails",
                column: "LoanDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewDetails_LoanId",
                table: "LoanReviewDetails",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewDetails_StatusId",
                table: "LoanReviewDetails",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewDetails_TemplateId",
                table: "LoanReviewDetails",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewItems_OptionId",
                table: "LoanReviewItems",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewItems_ReviewId",
                table: "LoanReviewItems",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviewItems_TemplateItemId",
                table: "LoanReviewItems",
                column: "TemplateItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanReviews_LoanId",
                table: "LoanReviews",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewOptions_OptionTypeId",
                table: "ReviewOptions",
                column: "OptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewTemplateItems_ItemOptionTypeId",
                table: "ReviewTemplateItems",
                column: "ItemOptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanReviewItems");

            migrationBuilder.DropTable(
                name: "LoanReviewReports");

            migrationBuilder.DropTable(
                name: "LoanReviewDetails");

            migrationBuilder.DropTable(
                name: "ReviewOptions");

            migrationBuilder.DropTable(
                name: "ReviewTemplateItems");

            migrationBuilder.DropTable(
                name: "LoanReviewStatuses");

            migrationBuilder.DropTable(
                name: "LoanReviews");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ReviewOptionTypes");

            migrationBuilder.DropTable(
                name: "ReviewTemplates");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
