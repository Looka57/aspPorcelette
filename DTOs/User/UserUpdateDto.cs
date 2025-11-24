using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http; // ⚠️ AJOUTÉ pour IFormFile

namespace ASPPorcelette.API.DTOs.User
{
    /// <summary>
    /// DTO utilisé pour la mise à jour du profil par l'utilisateur connecté ou par l'Admin/Sensei.
    /// Tous les champs sont optionnels car l'utilisateur ne les envoie pas tous à chaque fois.
    /// </summary>
    public class UserUpdateDto
    {
        public string? UserId { get; set; }
        
        // === IDENTITY ===
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Username { get; set; }

        // === INFORMATIONS PERSONNELLES ===
        [MaxLength(100)]
        public string? Nom { get; set; }

        [MaxLength(100)]
        public string? Prenom { get; set; }

        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string? Telephone { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        // === CHANGEMENT DE MOT DE PASSE (Optionnel) ===
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string? NewPassword { get; set; }

        // --- ADRESSE ---
        [MaxLength(200)]
        public string? RueEtNumero { get; set; }

        [MaxLength(100)]
        public string? Ville { get; set; }

        [MaxLength(10)]
        public string? CodePostal { get; set; }

        // === CHAMPS SPÉCIFIQUES SENSEI / ADMIN (Profil étendu) ===
        [MaxLength(50)]
        public string? Grade { get; set; }

        [MaxLength(4000)]
        public string? Bio { get; set; }

        public int? Statut { get; set; }

        public int? DisciplineId { get; set; }

        // === CHAMPS SPÉCIFIQUES ADHERENT / ADMIN (Dates) ===
    [DataType(DataType.Date)] // 👈 AJOUTER CECI
    public DateTime? DateDeNaissance { get; set; }
        
        public DateTime? DateAdhesion { get; set; } 
        public DateTime? DateRenouvellement { get; set; }

        // === FICHIER (Pour upload via FromForm) ===
        public IFormFile? PhotoFile { get; set; } 
    }
}