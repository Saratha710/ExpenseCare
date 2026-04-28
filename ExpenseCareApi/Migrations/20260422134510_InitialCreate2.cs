using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseCareApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DonorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonorAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DonorMobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DonationFor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiptIssuedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntryAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExpenseType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    EntryBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntryAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonationDetails");

            migrationBuilder.DropTable(
                name: "ExpenseDetails");
        }
    }
}
