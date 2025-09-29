using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class Apprendre
    {
        [Required]
        public int AdherentId { get; set; }

        [Required]
        public int DisciplineId { get; set; }

        [ForeignKey(nameof(AdherentId))]
        public Adherent AdherentApprenant { get; set; }

        [ForeignKey(nameof(DisciplineId))]
        public Discipline DisciplinePratiquee { get; set; }

        // Propriétés supplémentaires si besoin
        // public DateTime DateInscription { get; set; } = DateTime.Now;
    }
}
