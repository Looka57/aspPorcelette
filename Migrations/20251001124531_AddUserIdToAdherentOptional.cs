using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToAdherentOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Adherents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adherents_UserId",
                table: "Adherents",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Adherents_AspNetUsers_UserId",
                table: "Adherents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adherents_AspNetUsers_UserId",
                table: "Adherents");

            migrationBuilder.DropIndex(
                name: "IX_Adherents_UserId",
                table: "Adherents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Adherents");
        }
    }
}
