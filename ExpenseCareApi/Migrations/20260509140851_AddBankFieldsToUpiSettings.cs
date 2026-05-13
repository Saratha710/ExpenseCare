using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseCareApi.Migrations
{
    /// <inheritdoc />
    public partial class AddBankFieldsToUpiSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountHolderName",
                table: "UpiSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "UpiSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankIfscCode",
                table: "UpiSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "UpiSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountHolderName",
                table: "UpiSettings");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "UpiSettings");

            migrationBuilder.DropColumn(
                name: "BankIfscCode",
                table: "UpiSettings");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "UpiSettings");
        }
    }
}
