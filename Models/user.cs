using Microsoft.AspNetCore.Identity;

namespace ASPPorcelette.API.Models
{
    // C'est votre classe d'utilisateur principale pour l'authentification
    public class User : IdentityUser
    {
        // Propriétés personnelles ajoutées
        public string Nom { get; set; }
        public string Prenom { get; set; }

        // -----------------------------------------------------------------
        // Clé Étrangère (Foreign Key)
        // -----------------------------------------------------------------

        // Lien vers le profil détaillé du Sensei (nullable car l'utilisateur n'est pas forcément un Sensei)
        public int? SenseiId { get; set; }
        // Note : Vous n'avez pas besoin de [ForeignKey] si vous utilisez la convention de nommage par défaut (SenseiId)

        // -----------------------------------------------------------------
        // Propriété de Navigation (Lien)
        // -----------------------------------------------------------------

        // Permet d'accéder au profil Sensei détaillé
        public Sensei? SenseiProfil { get; set; }


    }
}
