using System.ComponentModel.DataAnnotations;
using System;

namespace ASPPorcelette.DTOs // Remplacez par votre namespace
{
    /// <summary>
    /// DTO utilisé pour la création unifiée d'un utilisateur ET de son profil métier (Adherent ou Sensei).
    /// </summary>
    public class UserCreationDto
    {
        // Propriétés requises pour le compte User (Identity)
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Le {0} doit faire au moins {2} caractères de long.", MinimumLength = 6)]
        public string Password { get; set; }

        // Propriétés communes (pour User et Profil)
        [Required]
        public string Prenom { get; set; }
        
        [Required]
        public string Nom { get; set; }

        // Propriété de décision pour le profil à créer
        [Required]
        public bool IsSensei { get; set; } 

        // Propriétés spécifiques aux profils métier (Adherent ou Sensei)
        // Ajoutez ici les autres champs pertinents que vous avez sur Sensei et Adherent
        public string Telephone { get; set; }
        public DateTime? DateAdhesion { get; set; } 
        public string Grade { get; set; } // Par exemple, "Kyu 4" ou "Ceinture Noire"
        public string Bio { get; set; }
    }
}
