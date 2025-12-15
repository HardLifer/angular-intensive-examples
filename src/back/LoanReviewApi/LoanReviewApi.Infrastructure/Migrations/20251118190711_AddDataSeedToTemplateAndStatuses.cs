using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeedToTemplateAndStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LoanReviewStatuses",
                columns: new[] { "Id", "created_at", "Status", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 18, 19, 7, 9, 941, DateTimeKind.Unspecified).AddTicks(6783), "In Review", null },
                    { 2, new DateTime(2025, 11, 18, 19, 7, 9, 941, DateTimeKind.Unspecified).AddTicks(7609), "Review Complete", null },
                    { 3, new DateTime(2025, 11, 18, 19, 7, 9, 941, DateTimeKind.Unspecified).AddTicks(7618), "New", null }
                });

            migrationBuilder.InsertData(
                table: "ReviewTemplates",
                columns: new[] { "Id", "created_at", "Name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 18, 19, 7, 9, 946, DateTimeKind.Unspecified).AddTicks(5053), "Residential", null },
                    { 2, new DateTime(2025, 11, 18, 19, 7, 9, 949, DateTimeKind.Unspecified).AddTicks(6118), "Commercial", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LoanReviewStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ReviewTemplates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReviewTemplates",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
