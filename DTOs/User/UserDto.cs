using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs
{
    public class UserDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Prenom { get; set; } = string.Empty;

        [Required]
        public string Nom { get; set; } = string.Empty;

        public string? Telephone { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime DateDeCreation { get; set; }
        public string? Grade { get; set; }
        public string? Statut { get; set; }
        public string? RueEtNumero { get; set; }
        public string? Ville { get; set; }
        public string? CodePostal { get; set; }
        public string? Bio { get; set; }
        public int? DisciplineId { get; set; } // Pour les Sensei
        public DateTime? DateNaissance { get; set; }
        public DateTime? DateAdhesion { get; set; }
        public DateTime? DateRenouvellement { get; set; }

        // Rôles de l'utilisateur
        public List<string> Roles { get; set; } = new List<string>();

        // ❌ SUPPRIMEZ ces deux lignes :
        // public SenseiDto? ProfilSensei { get; set; }
        // public AdherentDto? ProfilAdherent { get; set; }
    }
}