using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class BuildingTableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                table: "Flats");

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Flats",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BuildingNumber = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flats_BuildingId",
                table: "Flats",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Buildings_BuildingId",
                table: "Flats",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flats_Buildings_BuildingId",
                table: "Flats");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Flats_BuildingId",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Flats");

            migrationBuilder.AddColumn<int>(
                name: "BuildingNumber",
                table: "Flats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
