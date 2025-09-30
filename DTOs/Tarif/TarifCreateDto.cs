using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.Tarif
{
    public class TarifCreateDto
    {
        [Required(ErrorMessage = "Le nom du tarif est requis.")]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Le montant est requis.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à zéro.")]
        public decimal Montant { get; set; }

        [Required(ErrorMessage = "La périodicité est requise.")]
        [MaxLength(50)]
        public string Periodicite { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ID de la discipline est requis.")]
        public int DisciplineId { get; set; }

        public bool EstActif { get; set; } = true;
    }
}
