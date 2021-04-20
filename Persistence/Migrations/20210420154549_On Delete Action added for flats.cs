using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class OnDeleteActionaddedforflats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitImages_Flats_FlatId",
                table: "UnitImages");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitImages_Flats_FlatId",
                table: "UnitImages",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitImages_Flats_FlatId",
                table: "UnitImages");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitImages_Flats_FlatId",
                table: "UnitImages",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
