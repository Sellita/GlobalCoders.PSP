using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteForMerchant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Merchant",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Merchant");
        }
    }
}
