using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchOutTime",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "ClosingHour",
                table: "Merchant");

            migrationBuilder.DropColumn(
                name: "OpeningHour",
                table: "Merchant");

            migrationBuilder.CreateTable(
                name: "OrganizationScheduleEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MerchantEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationScheduleEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationScheduleEntity_Merchant_MerchantEntityId",
                        column: x => x.MerchantEntityId,
                        principalTable: "Merchant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationScheduleEntity_MerchantEntityId",
                table: "OrganizationScheduleEntity",
                column: "MerchantEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationScheduleEntity");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "BatchOutTime",
                table: "Merchant",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosingHour",
                table: "Merchant",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningHour",
                table: "Merchant",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
