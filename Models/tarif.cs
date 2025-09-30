using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class Tarif
    {
        [Key]
        public int TarifId { get; set; }

        [Required]
        [MaxLength(100)]
        // Ex: "Tarif Annuel Adulte", "Tarif Trimestriel Enfant"
        public string Nom { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Précision pour les montants monétaires
        public decimal Montant { get; set; }

        [Required]
        [MaxLength(50)]
        // Ex: "Annuel", "Trimestriel", "Mensuel", "Séance"
        public string Periodicite { get; set; }

        public bool EstActif { get; set; } = true;

        // -----------------------------------------------------------------
        // Clé Étrangère
        // -----------------------------------------------------------------

        [Required]
        // Le tarif est lié à une Discipline spécifique
        public int DisciplineId { get; set; }

        // -----------------------------------------------------------------
        // Propriétés de Navigation
        // -----------------------------------------------------------------

        [ForeignKey(nameof(DisciplineId))]
        public Discipline Discipline { get; set; } = null!;
    }
}
