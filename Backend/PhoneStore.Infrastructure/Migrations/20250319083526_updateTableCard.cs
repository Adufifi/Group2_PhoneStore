using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTableCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Cart_CartId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Product_ProductId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_ProductId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Account_CartId",
                table: "Account");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "Account",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CartId",
                table: "Product",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CartId",
                table: "Account",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Cart_CartId",
                table: "Account",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Cart_CartId",
                table: "Product",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Cart_CartId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Cart_CartId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CartId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Account_CartId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Product");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "Account",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductId",
                table: "Cart",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CartId",
                table: "Account",
                column: "CartId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Cart_CartId",
                table: "Account",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Product_ProductId",
                table: "Cart",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
