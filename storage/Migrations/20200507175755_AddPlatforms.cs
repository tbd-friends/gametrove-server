using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameTrove.Storage.Migrations
{
    public partial class AddPlatforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Registered",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "PlatformGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    GameId = table.Column<Guid>(nullable: false),
                    PlatformId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Registered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformGames");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Registered",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
