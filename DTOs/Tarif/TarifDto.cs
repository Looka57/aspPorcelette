using ASPPorcelette.API.DTOs.Discipline;
using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.Tarif
{
    public class TarifDto
    {
        public int TarifId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public string Periodicite { get; set; } = string.Empty;
        public bool EstActif { get; set; }
        
        // Inclut l'ID et les détails de la Discipline associée
        public int DisciplineId { get; set; }
        // Utilise le DTO existant pour la Discipline
        public DisciplineDto Discipline { get; set; } = null!; 
    }
}
