using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.Tarif
{
    public class TarifUpdateDto
    {
        [MaxLength(100)]
        public string? Nom { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Montant { get; set; }

        [MaxLength(50)]
        public string? Periodicite { get; set; }

        public int? DisciplineId { get; set; }

        public bool? EstActif { get; set; }
    }
}
