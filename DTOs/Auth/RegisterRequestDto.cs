using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.Models.Identity.Dto
{
    /// <summary>
    /// DTO pour la requête d'enregistrement d'un nouvel utilisateur
    /// </summary>
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Prenom { get; set; }
        
        [Required]
        public string Nom { get; set; }

        // --- NOUVELLES PROPRIÉTÉS REQUISES POUR L'INSCRIPTION ---

        public string Telephone { get; set; }
        
        public DateTime DateNaissance { get; set; }

        // Champs d'adresse
        public string RueEtNumero { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }

        // Champs spécifiques ou facultatifs
        public string Grade { get; set; }
        public string PhotoUrl { get; set; } 
        public string Bio { get; set; }
        
        public int Statut { get; set; } 
        public int DisciplineId { get; set; } // Gardé au cas où il est nécessaire
        
        // Champs de date d'adhésion (Souvent générés par le serveur, mais inclus si le client doit les fournir)
        public DateTime DateAdhesion { get; set; }
        public DateTime DateRenouvellement { get; set; }
    }
}
