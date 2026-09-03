using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ASPPorcelette.API.DTOs.User
{
    /// <summary>
    /// DTO utilisé pour la création d'un utilisateur
    /// et de son profil métier.
    /// </summary>
    public class UserCreationDto
    {
        // =========================
        // IDENTITY
        // =========================

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        [StringLength(
            100,
            ErrorMessage = "Le {0} doit faire au moins {2} caractères.",
            MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;


         // =========================
        // Roles
        // =========================
        public List<string> Roles { get; set; } = new();

        // =========================
        // INFORMATIONS
        // =========================

        [Required(ErrorMessage = "Le prénom est requis.")]
        [MaxLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis.")]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string? Telephone { get; set; }

        public IFormFile? PhotoFile { get; set; }

        // =========================
        // ADRESSE
        // =========================

        [Required(ErrorMessage = "L'adresse est requise.")]
        [MaxLength(200)]
        public string? RueEtNumero { get; set; }

        [Required(ErrorMessage = "La ville est requise.")]
        [MaxLength(100)]
        public string? Ville { get; set; }

        [Required(ErrorMessage = "Le code postal est requis.")]
        [MaxLength(10)]
        public string? CodePostal { get; set; }

        // =========================
        // SENSEI
        // =========================

        [MaxLength(50)]
        public string? Grade { get; set; }

        [MaxLength(4000)]
        public string? Bio { get; set; }

        public int? Statut { get; set; } = 0;

        public int? DisciplineId { get; set; }

        // =========================
        // ADHERENT
        // =========================

        public DateTime? DateNaissance { get; set; }

        public DateTime? DateAdhesion { get; set; }

        public DateTime? DateRenouvellement { get; set; }

        // =========================
// CERTIFICAT MEDICAL
// =========================
public bool CertificatMedicalFourni { get; set; }
public DateTime? DateCertificatMedical { get; set; }
    }
}