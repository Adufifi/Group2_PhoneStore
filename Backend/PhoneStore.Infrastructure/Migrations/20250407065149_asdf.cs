using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariant_Cart_CartId",
                table: "ProductVariant");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_CapacityId",
                table: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_CartId",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "ProductImageId",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brand");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ProductVariant",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariantsId",
                table: "Cart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_CapacityId",
                table: "ProductVariant",
                column: "CapacityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductVariantsId",
                table: "Cart",
                column: "ProductVariantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_ProductVariant_ProductVariantsId",
                table: "Cart",
                column: "ProductVariantsId",
                principalTable: "ProductVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_ProductVariant_ProductVariantsId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariant_CapacityId",
                table: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_Cart_ProductVariantsId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "ProductVariantsId",
                table: "Cart");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "ProductVariant",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductImageId",
                table: "ProductVariant",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brand",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductVariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Img = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_ProductVariant_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_CapacityId",
                table: "ProductVariant",
                column: "CapacityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_CartId",
                table: "ProductVariant",
                column: "CartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductVariantId",
                table: "ProductImage",
                column: "ProductVariantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariant_Cart_CartId",
                table: "ProductVariant",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
