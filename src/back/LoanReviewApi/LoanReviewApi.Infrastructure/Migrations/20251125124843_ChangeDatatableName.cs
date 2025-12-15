using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDatatableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanReviewDetails_LoanReviewStatuses_StatusId",
                table: "LoanReviewDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanReviewStatuses",
                table: "LoanReviewStatuses");

            migrationBuilder.RenameTable(
                name: "LoanReviewStatuses",
                newName: "LoanReviewStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanReviewStatus",
                table: "LoanReviewStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanReviewDetails_LoanReviewStatus_StatusId",
                table: "LoanReviewDetails",
                column: "StatusId",
                principalTable: "LoanReviewStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanReviewDetails_LoanReviewStatus_StatusId",
                table: "LoanReviewDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanReviewStatus",
                table: "LoanReviewStatus");

            migrationBuilder.RenameTable(
                name: "LoanReviewStatus",
                newName: "LoanReviewStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanReviewStatuses",
                table: "LoanReviewStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanReviewDetails_LoanReviewStatuses_StatusId",
                table: "LoanReviewDetails",
                column: "StatusId",
                principalTable: "LoanReviewStatuses",
                principalColumn: "Id");
        }
    }
}
