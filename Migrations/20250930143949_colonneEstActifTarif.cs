using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class colonneEstActifTarif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Disciplines_DisciplineId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tarifs");

            migrationBuilder.DropColumn(
                name: "Libelle",
                table: "Tarifs");

            migrationBuilder.RenameColumn(
                name: "Prix",
                table: "Tarifs",
                newName: "Montant");

            migrationBuilder.AlterColumn<int>(
                name: "DisciplineId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "EstActif",
                table: "Tarifs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Tarifs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Periodicite",
                table: "Tarifs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Disciplines_DisciplineId",
                table: "Transactions",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "DisciplineId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Disciplines_DisciplineId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EstActif",
                table: "Tarifs");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Tarifs");

            migrationBuilder.DropColumn(
                name: "Periodicite",
                table: "Tarifs");

            migrationBuilder.RenameColumn(
                name: "Montant",
                table: "Tarifs",
                newName: "Prix");

            migrationBuilder.AlterColumn<int>(
                name: "DisciplineId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tarifs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Libelle",
                table: "Tarifs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Disciplines_DisciplineId",
                table: "Transactions",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "DisciplineId");
        }
    }
}
