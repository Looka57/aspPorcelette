using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class Evenement
    {
        [Key]
        public int EvenementId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DateDebut { get; set; }
        
        // Optionnel si l'événement n'a pas toujours une fin définie (ex: journée complète)
        public DateTime? DateFin { get; set; }
        public string? ImageUrl { get; set; }

        // --- Clé Étrangère vers TypeEvenement ---
        // L'ID du Type (Ex: Conférence, Assemblée, Tournoi, Fête)
        [Required]
        public int TypeEvenementId { get; set; }

        // Propriété de navigation : Un événement a un seul TypeEvenement
        [ForeignKey("TypeEvenementId")]
        public TypeEvenement TypeEvenement { get; set; } = null!;


        // --- Clé Étrangère vers Discipline (si nécessaire pour un lien direct) ---
        // J'ajoute cette clé au cas où l'événement est spécifique à une discipline (Ex: Tournoi de Judo)
        public int? DisciplineId { get; set; }
        
        // Propriété de navigation vers la Discipline (peut être null)
        [ForeignKey("DisciplineId")]
        public Discipline? Discipline { get; set; }


        // Note : D'autres clés (ex: LieuId, UtilisateurId) pourraient être ajoutées ici.
    }
}

