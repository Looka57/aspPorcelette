using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.DTOs.Adherent;
using ASPPorcelette.API.DTOs.Sensei;

namespace ASPPorcelette.API.DTOs
{
    // Utilisé pour retourner les informations de base de l'utilisateur
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
        public string? Adresse { get; set; }
        public string? Ville { get; set; }
        public string? CodePostal { get; set; }
        public string? Bio { get; set; }

        // Rôles de l'utilisateur
        public List<string> Roles { get; set; } = new List<string>();

        // Profil Sensei (si l'utilisateur est Sensei)
        public SenseiDto? ProfilSensei { get; set; }

        // Profil Adherent (si l'utilisateur est Adherent)
        public AdherentDto? ProfilAdherent { get; set; }

        public int? DisciplineId { get; set; } 
    }
}
