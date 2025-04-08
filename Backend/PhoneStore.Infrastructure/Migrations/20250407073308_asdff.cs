using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariant_Color_ProductColorId",
                table: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_ProductColorId",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "ProductColorId",
                table: "ProductVariant");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ColorId",
                table: "ProductVariant",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariant_Color_ColorId",
                table: "ProductVariant",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariant_Color_ColorId",
                table: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_ColorId",
                table: "ProductVariant");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductColorId",
                table: "ProductVariant",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ProductColorId",
                table: "ProductVariant",
                column: "ProductColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariant_Color_ProductColorId",
                table: "ProductVariant",
                column: "ProductColorId",
                principalTable: "Color",
                principalColumn: "Id");
        }
    }
}
