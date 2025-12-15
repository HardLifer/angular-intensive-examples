using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EffetiveRate",          // old column name
                table: "LoanDetails", // replace with your actual table name
                newName: "EffectiveRate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EffectiveRate",          // old column name
                table: "LoanDetails", // replace with your actual table name
                newName: "EffetiveRate");
        }
    }
}
