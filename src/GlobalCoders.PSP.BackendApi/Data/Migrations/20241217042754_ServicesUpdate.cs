using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ServicesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Merchant_MerchantId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "MerchantId",
                table: "Services",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_MerchantId",
                table: "Services",
                newName: "IX_Services_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_EmployeeId",
                table: "Services",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_EmployeeId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Services",
                newName: "MerchantId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_EmployeeId",
                table: "Services",
                newName: "IX_Services_MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Merchant_MerchantId",
                table: "Services",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
