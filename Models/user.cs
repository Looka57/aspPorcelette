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
// Informations d'identification/personnelles communes
        public string Telephone { get; set; }
        
        // La date de naissance est essentielle pour l'âge et le statut d'adhésion
        public DateTime? DateNaissance { get; set; } // Nullable, car il peut ne pas être connu immédiatement
        
        // Adresse (composée, pour une saisie plus simple)
        public string RueEtNumero { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }
        
        // Détails spécifiques à l'activité (utilisés par Adhérents et Sensei)
        public string Grade { get; set; } // Exemple : Ceinture Noire, 1er Dan, 5e Kyu, etc.
        public string PhotoUrl { get; set; } // URL vers l'image du profil
        
        // Informations relatives à l'adhésion
        public DateTime? DateAdhesion { get; set; }
        public DateTime? DateRenouvellement { get; set; }
        
        // Le statut pourrait être une énumération (Actif, Inactif, Suspendu)
        public int Statut { get; set; } // 0: Inactif, 1: Actif, etc.
        
        // La bio est utile pour les Sensei
        public string Bio { get; set; }

          // AJOUT CRITIQUE pour résoudre l'erreur CS0117 dans SenseiService
        public int? DisciplineId { get; set; }
        
        [Required]
        public DateTime DateCreation { get; set; }
    }
}
