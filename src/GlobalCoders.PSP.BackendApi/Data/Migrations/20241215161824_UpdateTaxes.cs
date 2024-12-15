using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPayments_Orders_OrderEntityId",
                table: "OrderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderEntityId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OrderEntityId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderPayments_OrderEntityId",
                table: "OrderPayments");

            migrationBuilder.DropColumn(
                name: "DayOfMonth",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "Hour",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "Minute",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "OrderPayments");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductTypeId",
                table: "Tax",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderProductTaxEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductTaxEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProductTaxEntity_OrderProducts_OrderProductId",
                        column: x => x.OrderProductId,
                        principalTable: "OrderProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tax_ProductTypeId",
                table: "Tax",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_OrderId",
                table: "OrderPayments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductTaxEntity_OrderProductId",
                table: "OrderProductTaxEntity",
                column: "OrderProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPayments_Orders_OrderId",
                table: "OrderPayments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tax_ProductType_ProductTypeId",
                table: "Tax",
                column: "ProductTypeId",
                principalTable: "ProductType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPayments_Orders_OrderId",
                table: "OrderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tax_ProductType_ProductTypeId",
                table: "Tax");

            migrationBuilder.DropTable(
                name: "OrderProductTaxEntity");

            migrationBuilder.DropIndex(
                name: "IX_Tax_ProductTypeId",
                table: "Tax");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderPayments_OrderId",
                table: "OrderPayments");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "Tax");

            migrationBuilder.AddColumn<string>(
                name: "DayOfMonth",
                table: "Tax",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Tax",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hour",
                table: "Tax",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Minute",
                table: "Tax",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "Tax",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderEntityId",
                table: "OrderProducts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "OrderProducts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderEntityId",
                table: "OrderPayments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderEntityId",
                table: "OrderProducts",
                column: "OrderEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_OrderEntityId",
                table: "OrderPayments",
                column: "OrderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPayments_Orders_OrderEntityId",
                table: "OrderPayments",
                column: "OrderEntityId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderEntityId",
                table: "OrderProducts",
                column: "OrderEntityId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
