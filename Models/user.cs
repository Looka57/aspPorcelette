using Microsoft.AspNetCore.Identity;

namespace ASPPorcelette.API.Models
{
    // C'est votre classe d'utilisateur principale pour l'authentification
    public class User : IdentityUser
    {
        // -----------------------------------------------------------------
        // Clé Étrangère (Foreign Key)
        // -----------------------------------------------------------------
        
        // Lien vers le profil détaillé du Sensei (nullable car l'utilisateur n'est pas forcément un Sensei)
        public int? SenseiId { get; set; } 

        // -----------------------------------------------------------------
        // Propriété de Navigation (Lien)
        // -----------------------------------------------------------------
        
        // Permet d'accéder au profil Sensei détaillé         public Sensei? SenseiProfil { get; set; }
    }
}