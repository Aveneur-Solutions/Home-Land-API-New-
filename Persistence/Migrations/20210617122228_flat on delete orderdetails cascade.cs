using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class flatondeleteorderdetailscascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Flats_FlatId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_FlatId",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_FlatId",
                table: "OrderDetails",
                column: "FlatId",
                unique: true,
                filter: "[FlatId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Flats_FlatId",
                table: "OrderDetails",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Flats_FlatId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_FlatId",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_FlatId",
                table: "OrderDetails",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Flats_FlatId",
                table: "OrderDetails",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
