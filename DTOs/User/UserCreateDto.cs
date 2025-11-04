using System.ComponentModel.DataAnnotations;
using System;

namespace ASPPorcelette.API.DTOs.User
{
    /// <summary>
    /// DTO utilisé pour la création unifiée d'un utilisateur ET de son profil métier (Adherent ou Sensei), 
    /// car toutes les données sont stockées dans l'entité Identity 'User'.
    /// </summary>
    public class UserCreationDto
    {
        // === INFORMATIONS IDENTITY (Obligatoires) ===
        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Le {0} doit faire au moins {2} caractères.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        // === INFORMATIONS COMMUNES (Obligatoires) ===
        [Required(ErrorMessage = "Le prénom est requis.")]
        [MaxLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis.")]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        // // === CHOIX DU TYPE DE PROFIL ===
        // [Required(ErrorMessage = "Le type de profil est requis.")]
        // public bool IsSensei { get; set; }

        // === CHAMPS COMMUNS & ADRESSE (Optionnels ou avec valeurs par défaut) ===
        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string? Telephone { get; set; }

        public IFormFile? PhotoFile { get; set; }

        // --- ADRESSE (Requise pour l'adhérent) ---
        [Required(ErrorMessage = "L'adresse est requise.")]
        [MaxLength(200)]
        public string? RueEtNumero { get; set; } // Mappé à User.RueEtNumero
        
        [Required(ErrorMessage = "La ville est requise.")]
        [MaxLength(100)]
        public string? Ville { get; set; } // <-- AJOUTÉ
        
        [Required(ErrorMessage = "Le code postal est requis.")]
        [MaxLength(10)]
        public string? CodePostal { get; set; } // <-- AJOUTÉ

        // === CHAMPS SPÉCIFIQUES SENSEI ===
        [MaxLength(50)]
        public string? Grade { get; set; }

        [MaxLength(4000)]
        public string? Bio { get; set; }

        public int? Statut { get; set; } = 0;

        // CORRECTION : Discipline obligatoire pour Sensei
        public int? DisciplineId { get; set; }

        // === CHAMPS SPÉCIFIQUES ADHERENT ===
        public DateTime? DateNaissance { get; set; }
        public DateTime? DateAdhesion { get; set; }
        public DateTime? DateRenouvellement { get; set; }
    }
}
