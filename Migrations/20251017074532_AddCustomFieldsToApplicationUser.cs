using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomFieldsToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actualites_Senseis_SenseiId",
                table: "Actualites");

            migrationBuilder.DropForeignKey(
                name: "FK_Cours_Senseis_SenseiId",
                table: "Cours");

            migrationBuilder.DropForeignKey(
                name: "FK_Senseis_AspNetUsers_UserId",
                table: "Senseis");

            migrationBuilder.DropForeignKey(
                name: "FK_Senseis_Disciplines_DisciplineId",
                table: "Senseis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Senseis",
                table: "Senseis");

            migrationBuilder.RenameTable(
                name: "Senseis",
                newName: "Sensei");

            migrationBuilder.RenameIndex(
                name: "IX_Senseis_UserId",
                table: "Sensei",
                newName: "IX_Sensei_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Senseis_DisciplineId",
                table: "Sensei",
                newName: "IX_Sensei_DisciplineId");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodePostal",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdhesion",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateNaissance",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRenouvellement",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RueEtNumero",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensei",
                table: "Sensei",
                column: "SenseiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actualites_Sensei_SenseiId",
                table: "Actualites",
                column: "SenseiId",
                principalTable: "Sensei",
                principalColumn: "SenseiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cours_Sensei_SenseiId",
                table: "Cours",
                column: "SenseiId",
                principalTable: "Sensei",
                principalColumn: "SenseiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensei_AspNetUsers_UserId",
                table: "Sensei",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensei_Disciplines_DisciplineId",
                table: "Sensei",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "DisciplineId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actualites_Sensei_SenseiId",
                table: "Actualites");

            migrationBuilder.DropForeignKey(
                name: "FK_Cours_Sensei_SenseiId",
                table: "Cours");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensei_AspNetUsers_UserId",
                table: "Sensei");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensei_Disciplines_DisciplineId",
                table: "Sensei");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensei",
                table: "Sensei");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CodePostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateAdhesion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateNaissance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateRenouvellement",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RueEtNumero",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Sensei",
                newName: "Senseis");

            migrationBuilder.RenameIndex(
                name: "IX_Sensei_UserId",
                table: "Senseis",
                newName: "IX_Senseis_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Sensei_DisciplineId",
                table: "Senseis",
                newName: "IX_Senseis_DisciplineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Senseis",
                table: "Senseis",
                column: "SenseiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actualites_Senseis_SenseiId",
                table: "Actualites",
                column: "SenseiId",
                principalTable: "Senseis",
                principalColumn: "SenseiId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cours_Senseis_SenseiId",
                table: "Cours",
                column: "SenseiId",
                principalTable: "Senseis",
                principalColumn: "SenseiId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Senseis_AspNetUsers_UserId",
                table: "Senseis",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Senseis_Disciplines_DisciplineId",
                table: "Senseis",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "DisciplineId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
