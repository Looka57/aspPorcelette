using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.Models.Identity
{
    // Le modèle User hérite des fonctionnalités de base de l'authentification (Id, Email, UserName, HashPassword, etc.)
    public class User : IdentityUser
    {
        // Propriétés de base de l'utilisateur (utiles même s'il n'est pas Sensei)

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; }

        [Required]
        [MaxLength(100)]
        public string Prenom { get; set; }

        // --- RELATIONS ---
        
        [Required]
        public DateTime DateCreation { get; set; }
    }
}
