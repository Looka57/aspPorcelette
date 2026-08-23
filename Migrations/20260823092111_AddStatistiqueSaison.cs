using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStatistiqueSaison : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatistiquesSaisons",
                columns: table => new
                {
                    StatistiqueSaisonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Saison = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    DisciplineId = table.Column<int>(type: "int", nullable: false),
                    TotalInscrits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesSaisons", x => x.StatistiqueSaisonId);
                    table.ForeignKey(
                        name: "FK_StatistiquesSaisons_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "DisciplineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatistiquesSaisons_DisciplineId",
                table: "StatistiquesSaisons",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_StatistiquesSaisons_Saison_DisciplineId",
                table: "StatistiquesSaisons",
                columns: new[] { "Saison", "DisciplineId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatistiquesSaisons");
        }
    }
}
