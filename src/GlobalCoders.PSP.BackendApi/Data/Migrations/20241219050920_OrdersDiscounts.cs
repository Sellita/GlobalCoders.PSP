using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrdersDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDiscountsEntity_Orders_OrderEntityId",
                table: "OrderDiscountsEntity");

            migrationBuilder.DropIndex(
                name: "IX_OrderDiscountsEntity_OrderEntityId",
                table: "OrderDiscountsEntity");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "OrderDiscountsEntity");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDiscountsEntity_OrderDiscountId",
                table: "OrderDiscountsEntity",
                column: "OrderDiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDiscountsEntity_Orders_OrderDiscountId",
                table: "OrderDiscountsEntity",
                column: "OrderDiscountId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDiscountsEntity_Orders_OrderDiscountId",
                table: "OrderDiscountsEntity");

            migrationBuilder.DropIndex(
                name: "IX_OrderDiscountsEntity_OrderDiscountId",
                table: "OrderDiscountsEntity");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderEntityId",
                table: "OrderDiscountsEntity",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDiscountsEntity_OrderEntityId",
                table: "OrderDiscountsEntity",
                column: "OrderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDiscountsEntity_Orders_OrderEntityId",
                table: "OrderDiscountsEntity",
                column: "OrderEntityId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
