
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class StatistiqueSaison
    {
        public int StatistiqueSaisonId { get; set; }

        [Required]
        [MaxLength(9)]
        public string Saison { get; set; }

        [Required]
        public int DisciplineId { get; set; }

        [Required]
        public int TotalInscrits { get; set; }

        // Navigation vers la discipline
        [ForeignKey(nameof(DisciplineId))]
        public Discipline Discipline { get; set; }
    }
}

