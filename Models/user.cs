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

        // Propriété de navigation pour le profil Sensei (Relation One-to-One)
        // La clé étrangère (UserId) est définie sur l'entité Sensei elle-même.
        public Sensei? Sensei { get; set; }
        
        [Required]
        public DateTime DateCreation { get; set; }
    }
}
