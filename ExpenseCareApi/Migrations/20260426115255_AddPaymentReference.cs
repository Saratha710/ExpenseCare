using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseCareApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentReference",
                table: "DonationDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentReference",
                table: "DonationDetails");
        }
    }
}
