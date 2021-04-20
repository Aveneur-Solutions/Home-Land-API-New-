using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class UpdatedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitImages_Flats_FlatId1",
                table: "UnitImages");

            migrationBuilder.DropIndex(
                name: "IX_UnitImages_FlatId1",
                table: "UnitImages");

            migrationBuilder.DropColumn(
                name: "FlatId1",
                table: "UnitImages");

            migrationBuilder.AlterColumn<string>(
                name: "FlatId",
                table: "UnitImages",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ImageLocation = table.Column<string>(nullable: true),
                    Section = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitImages_FlatId",
                table: "UnitImages",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitImages_Flats_FlatId",
                table: "UnitImages",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitImages_Flats_FlatId",
                table: "UnitImages");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_UnitImages_FlatId",
                table: "UnitImages");

            migrationBuilder.AlterColumn<Guid>(
                name: "FlatId",
                table: "UnitImages",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlatId1",
                table: "UnitImages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitImages_FlatId1",
                table: "UnitImages",
                column: "FlatId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitImages_Flats_FlatId1",
                table: "UnitImages",
                column: "FlatId1",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
