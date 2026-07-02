using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECE.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedInventoryItemTransactionAuditable_AddedPickingOrderToStorageLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PickSequence",
                table: "StorageLocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "InventoryItemTransactions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "InventoryItemTransactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "InventoryItemTransactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedUtc",
                table: "InventoryItemTransactions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocations_PickSequence",
                table: "StorageLocations",
                column: "PickSequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StorageLocations_PickSequence",
                table: "StorageLocations");

            migrationBuilder.DropColumn(
                name: "PickSequence",
                table: "StorageLocations");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "InventoryItemTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "InventoryItemTransactions");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "InventoryItemTransactions");

            migrationBuilder.DropColumn(
                name: "LastModifiedUtc",
                table: "InventoryItemTransactions");
        }
    }
}
