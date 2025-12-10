using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LoanDetailPossibleNullForReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoanDetailId",
                table: "LoanReviewDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ReviewTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ReviewTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 11, 19, 9, 25, 58, 608, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoanDetailId",
                table: "LoanReviewDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 11, 18, 19, 7, 9, 941, DateTimeKind.Unspecified).AddTicks(6783));

            migrationBuilder.UpdateData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 11, 18, 19, 7, 9, 941, DateTimeKind.Unspecified).AddTicks(7609));

            migrationBuilder.UpdateData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 11, 18, 19, 7, 9, 941, DateTimeKind.Unspecified).AddTicks(7618));

            migrationBuilder.UpdateData(
                table: "ReviewTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 11, 18, 19, 7, 9, 946, DateTimeKind.Unspecified).AddTicks(5053));

            migrationBuilder.UpdateData(
                table: "ReviewTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 11, 18, 19, 7, 9, 949, DateTimeKind.Unspecified).AddTicks(6118));
        }
    }
}
