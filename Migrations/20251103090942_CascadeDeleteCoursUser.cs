using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteCoursUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cours_AspNetUsers_UserId",
                table: "Cours");

            migrationBuilder.AddForeignKey(
                name: "FK_Cours_AspNetUsers_UserId",
                table: "Cours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cours_AspNetUsers_UserId",
                table: "Cours");

            migrationBuilder.AddForeignKey(
                name: "FK_Cours_AspNetUsers_UserId",
                table: "Cours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
