using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameTrove.Storage.Migrations
{
    public partial class AddPurchaseDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Purchased",
                table: "Copies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purchased",
                table: "Copies");
        }
    }
}
