using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECE.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedInventoryItem_Batch_Warehouse_StorageLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "InventoryItemSerialSequence");

            migrationBuilder.CreateTable(
                name: "InventoryItemsBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManufacturingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Lot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemsBatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemperatureRequirement = table.Column<int>(type: "int", nullable: true),
                    HumidityRequirement = table.Column<int>(type: "int", nullable: true),
                    LightRequirement = table.Column<int>(type: "int", nullable: true),
                    EnvironmentRequirement = table.Column<int>(type: "int", nullable: true),
                    SecurityRequirement = table.Column<int>(type: "int", nullable: true),
                    SafetyRequirement = table.Column<int>(type: "int", nullable: true),
                    CertificationRequirement = table.Column<int>(type: "int", nullable: true),
                    StorageEquipmentRequirement = table.Column<int>(type: "int", nullable: true),
                    OrientationRequirement = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageLocations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    StorageLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "'INV-' + RIGHT(REPLICATE('0', 10) + CAST(NEXT VALUE FOR InventoryItemSerialSequence AS varchar(10)), 10)"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_InventoryItemsBatches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "InventoryItemsBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Skus_SkuId",
                        column: x => x.SkuId,
                        principalTable: "Skus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryItems_StorageLocations_StorageLocationId",
                        column: x => x.StorageLocationId,
                        principalTable: "StorageLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_BatchId",
                table: "InventoryItems",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SerialNumber",
                table: "InventoryItems",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SkuId",
                table: "InventoryItems",
                column: "SkuId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_StorageLocationId",
                table: "InventoryItems",
                column: "StorageLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemsBatches_Lot",
                table: "InventoryItemsBatches",
                column: "Lot");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocations_Code_WarehouseId",
                table: "StorageLocations",
                columns: new[] { "Code", "WarehouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocations_WarehouseId",
                table: "StorageLocations",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Name",
                table: "Warehouses",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "InventoryItemsBatches");

            migrationBuilder.DropTable(
                name: "StorageLocations");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropSequence(
                name: "InventoryItemSerialSequence");
        }
    }
}
