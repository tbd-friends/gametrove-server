using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameTrove.Storage.Migrations
{
    public partial class GamePricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamePricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PriceChartingId = table.Column<int>(nullable: false),
                    CompleteInBoxPrice = table.Column<decimal>(nullable: false),
                    LoosePrice = table.Column<decimal>(nullable: false),
                    MatchConfidence = table.Column<short>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePricing", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePricing");
        }
    }
}
