using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECE.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSku_SkuDefiniion_SkuDefintionValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkuAttributeDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkuAttributeDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    SellingPriceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SellingPriceCurrency = table.Column<int>(type: "int", nullable: false),
                    WeightValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    WeightUnit = table.Column<int>(type: "int", nullable: true),
                    DimensionLength = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    DimensionWidth = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    DimensionHeight = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    DimensionLengthUnit = table.Column<int>(type: "int", nullable: true),
                    DefaultImageRelativePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsAvailableForShipment = table.Column<bool>(type: "bit", nullable: false),
                    ShelfLifeInDays = table.Column<int>(type: "int", nullable: true),
                    RequiredTemperature = table.Column<int>(type: "int", nullable: true),
                    RequiredHumidity = table.Column<int>(type: "int", nullable: true),
                    RequiredLight = table.Column<int>(type: "int", nullable: true),
                    RequiredEnvironment = table.Column<int>(type: "int", nullable: true),
                    RequiredSecurity = table.Column<int>(type: "int", nullable: true),
                    RequiredSafety = table.Column<int>(type: "int", nullable: true),
                    RequiredCertification = table.Column<int>(type: "int", nullable: true),
                    RequiredStorageEquipment = table.Column<int>(type: "int", nullable: true),
                    RequiredOrientation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skus_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkuAttributeValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkuAttributeDefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkuAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkuAttributeValues_SkuAttributeDefinitions_SkuAttributeDefinitionId",
                        column: x => x.SkuAttributeDefinitionId,
                        principalTable: "SkuAttributeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkuAttributeValues_Skus_SkuId",
                        column: x => x.SkuId,
                        principalTable: "Skus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkuAttributeDefinitions_Name",
                table: "SkuAttributeDefinitions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkuAttributeValues_SkuAttributeDefinitionId",
                table: "SkuAttributeValues",
                column: "SkuAttributeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_SkuAttributeValues_SkuId_SkuAttributeDefinitionId",
                table: "SkuAttributeValues",
                columns: new[] { "SkuId", "SkuAttributeDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skus_Barcode",
                table: "Skus",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skus_ProductId_Code",
                table: "Skus",
                columns: new[] { "ProductId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkuAttributeValues");

            migrationBuilder.DropTable(
                name: "SkuAttributeDefinitions");

            migrationBuilder.DropTable(
                name: "Skus");
        }
    }
}
