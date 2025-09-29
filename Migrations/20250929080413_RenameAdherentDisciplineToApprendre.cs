using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameAdherentDisciplineToApprendre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                   name: "AdherentDiscipline",
                   newName: "Apprendre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Apprendre",
                newName: "AdherentDiscipline");
        }
    }
}
