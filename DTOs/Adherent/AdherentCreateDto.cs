using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.Adherent
{
    public class AdherentCreateDto
    {
        [Required(ErrorMessage = "Le nom est requis.")]
        [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [MaxLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "La date de naissance est requise.")]
        public DateTime DateDeNaissance { get; set; }

        [Required(ErrorMessage = "L'adresse est requise.")]
        [MaxLength(200, ErrorMessage = "L'adresse ne peut pas dépasser 200 caractères.")]
        public string Adresse { get; set; }

        public string CodePostal { get; set; }
        public string Ville { get; set; }

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Le téléphone est requis.")]
        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string Telephone { get; set; }
        public DateTime DateAdhesion { get; set; } = DateTime.UtcNow;
        public DateTime DateRenouvellement { get; set; } = DateTime.UtcNow.AddYears(1);
        public string Statut { get; set; } = "Actif";
        public int? DisciplineId { get; set; } // nullable si pas obligatoire

    }
}