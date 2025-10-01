using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDateCreationToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Senseis_AspNetUsers_UtilisateurId1",
                table: "Senseis");

            migrationBuilder.DropColumn(
                name: "UtilisateurId",
                table: "Senseis");

            migrationBuilder.DropColumn(
                name: "SenseiId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UtilisateurId1",
                table: "Senseis",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Senseis_UtilisateurId1",
                table: "Senseis",
                newName: "IX_Senseis_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Prenom",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Senseis_AspNetUsers_UserId",
                table: "Senseis",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Senseis_AspNetUsers_UserId",
                table: "Senseis");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Senseis",
                newName: "UtilisateurId1");

            migrationBuilder.RenameIndex(
                name: "IX_Senseis_UserId",
                table: "Senseis",
                newName: "IX_Senseis_UtilisateurId1");

            migrationBuilder.AddColumn<string>(
                name: "UtilisateurId",
                table: "Senseis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Prenom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "SenseiId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Senseis_AspNetUsers_UtilisateurId1",
                table: "Senseis",
                column: "UtilisateurId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
