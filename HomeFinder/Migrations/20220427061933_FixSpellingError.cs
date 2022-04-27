using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeFinder.Migrations
{
    public partial class FixSpellingError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Adresses_AdressId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "AdressId",
                table: "Properties",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_AdressId",
                table: "Properties",
                newName: "IX_Properties_AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Adresses_AddressId",
                table: "Properties",
                column: "AddressId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Adresses_AddressId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Properties",
                newName: "AdressId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_AddressId",
                table: "Properties",
                newName: "IX_Properties_AdressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Adresses_AdressId",
                table: "Properties",
                column: "AdressId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
