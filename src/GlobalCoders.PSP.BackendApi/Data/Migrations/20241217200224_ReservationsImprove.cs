using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReservationsImprove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Reservations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Reservations");
        }
    }
}
