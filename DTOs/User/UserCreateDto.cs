using System.ComponentModel.DataAnnotations;
using System;

namespace ASPPorcelette.DTOs // Remplacez par votre namespace
{
    /// <summary>
    /// DTO utilisé pour la création unifiée d'un utilisateur ET de son profil métier (Adherent ou Sensei).
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

        // === CHOIX DU TYPE DE PROFIL ===
        [Required(ErrorMessage = "Le type de profil est requis.")]
        public bool IsSensei { get; set; }

        // === CHAMPS COMMUNS (Optionnels ou avec valeurs par défaut) ===
        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string? Telephone { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        // === CHAMPS SPÉCIFIQUES SENSEI ===
        [MaxLength(50)]
        public string? Grade { get; set; }

        [MaxLength(4000)]
        public string? Bio { get; set; }

        [MaxLength(50)]
        public string? Statut { get; set; } = "Actif";

        // CORRECTION : Discipline obligatoire pour Sensei
        public int? DisciplineId { get; set; }

        // === CHAMPS SPÉCIFIQUES ADHERENT ===
        public DateTime? DateDeNaissance { get; set; }

        [MaxLength(200)]
        public string? Adresse { get; set; }

        public DateTime? DateAdhesion { get; set; }

        public DateTime? DateRenouvellement { get; set; }
    }
}
