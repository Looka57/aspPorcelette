

using System.ComponentModel.DataAnnotations;
using System;

namespace ASPPorcelette.API.DTOs.User
{
    public class UserAdminUpdateDto // 👈 NOUVEAU DTO pour la mise à jour par l'admin
    {
        public string? UserId { get; set; } // L'ID pour identifier l'objet à modifier

        // === IDENTITY & INFO PERSONNELLES ===
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Username { get; set; } // Même si non utilisé dans le formulaire licencié, l'inclure si c'est la propriété UserName dans la DB

        [MaxLength(100)]
        public string? Nom { get; set; }
        
        [MaxLength(100)]
        public string? Prenom { get; set; }

        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string? Telephone { get; set; }
        
        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        // ❌ CurrentPassword et NewPassword sont DÉLIBÉRÉMENT exclus ici.

        // --- ADRESSE ---
        [MaxLength(200)]
        public string? Adresse { get; set; }
        [MaxLength(100)]
        public string? Ville { get; set; } 
        [MaxLength(10)]
        public string? CodePostal { get; set; } 

        // === AUTRES CHAMPS ===
        [MaxLength(50)]
        public string? Grade { get; set; }

        [MaxLength(4000)]
        public string? Bio { get; set; }
        
        // 💡 Le statut est un entier dans votre Swagger
        public int? Statut { get; set; } = 0; 

        public int? DisciplineId { get; set; }

        public DateTime? DateDeNaissance { get; set; }
        public DateTime? DateAdhesion { get; set; } // Ajouter ces dates si elles sont dans le modèle
        public DateTime? DateRenouvellement { get; set; }
    }
}