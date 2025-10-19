using System.ComponentModel.DataAnnotations;
using System;

namespace ASPPorcelette.API.DTOs.User
{
    /// <summary>
    /// DTO utilisé pour la mise à jour du profil par l'utilisateur connecté.
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

        // --- ADRESSE (POUR LE MODÈLE UNIFIÉ) ---
        [MaxLength(200)]
        public string? Adresse { get; set; } // Mappé à User.RueEtNumero


        [MaxLength(100)]
        public string? Ville { get; set; } // <-- AJOUTÉ

        [MaxLength(10)]
        public string? CodePostal { get; set; } // <-- AJOUTÉ

        // === CHAMPS SPÉCIFIQUES SENSEI ===
        [MaxLength(50)]
        public string? Grade { get; set; }

        [MaxLength(4000)]
        public string? Bio { get; set; }

        public int? Statut { get; set; } = 0;

        public int? DisciplineId { get; set; }

        // === CHAMPS SPÉCIFIQUES ADHERENT ===
        public DateTime? DateDeNaissance { get; set; }
    }
}
