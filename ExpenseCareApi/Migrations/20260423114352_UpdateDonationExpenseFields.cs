using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseCareApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDonationExpenseFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "PaymentReference",
                table: "DonationDetails");

            migrationBuilder.DropColumn(
                name: "ReceiptIssuedAt",
                table: "DonationDetails");

            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "DonationDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ExpenseDetails",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AttachImage",
                table: "ExpenseDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ExpenseDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "DonorMobile",
                table: "DonationDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DonationDate",
                table: "DonationDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedAt",
                table: "DonationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "DonationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DonationDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachImage",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "DonationDetails");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "DonationDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DonationDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ExpenseDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "ExpenseDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "ExpenseDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ExpenseDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "ExpenseDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonorMobile",
                table: "DonationDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DonationDate",
                table: "DonationDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReference",
                table: "DonationDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiptIssuedAt",
                table: "DonationDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptNumber",
                table: "DonationDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
