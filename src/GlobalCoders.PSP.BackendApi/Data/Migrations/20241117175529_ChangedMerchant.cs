using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMerchant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperatingHour",
                table: "Merchant",
                newName: "OpeningHour");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpeningHour",
                table: "Merchant",
                newName: "OperatingHour");
        }
    }
}
