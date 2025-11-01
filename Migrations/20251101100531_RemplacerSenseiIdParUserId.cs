using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPPorcelette.API.Migrations
{
    /// <inheritdoc />
    public partial class RemplacerSenseiIdParUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Supprimer la contrainte étrangère existante vers Sensei
            migrationBuilder.DropForeignKey(
                name: "FK_Cours_Sensei_SenseiId",
                table: "Cours");

            // 2️⃣ Supprimer l'index lié à SenseiId
            migrationBuilder.DropIndex(
                name: "IX_Cours_SenseiId",
                table: "Cours");

            // 3️⃣ Renommer la colonne SenseiId en UserId
            migrationBuilder.RenameColumn(
                name: "SenseiId",
                table: "Cours",
                newName: "UserId");

            // 4️⃣ Modifier le type de la colonne UserId (int → nvarchar(450))
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Cours",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // 5️⃣ Créer un nouvel index sur UserId
            migrationBuilder.CreateIndex(
                name: "IX_Cours_UserId",
                table: "Cours",
                column: "UserId");

            // 6️⃣ Ajouter la nouvelle clé étrangère vers AspNetUsers.Id
            migrationBuilder.AddForeignKey(
                name: "FK_Cours_AspNetUsers_UserId",
                table: "Cours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Supprimer la FK et l'index nouvellement créés
            migrationBuilder.DropForeignKey(
                name: "FK_Cours_AspNetUsers_UserId",
                table: "Cours");

            migrationBuilder.DropIndex(
                name: "IX_Cours_UserId",
                table: "Cours");

            // 2️⃣ Revenir au type int
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Cours",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // 3️⃣ Renommer UserId → SenseiId
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Cours",
                newName: "SenseiId");

            // 4️⃣ Recréer l'index et la FK vers Sensei
            migrationBuilder.CreateIndex(
                name: "IX_Cours_SenseiId",
                table: "Cours",
                column: "SenseiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cours_Sensei_SenseiId",
                table: "Cours",
                column: "SenseiId",
                principalTable: "Sensei",
                principalColumn: "SenseiId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
