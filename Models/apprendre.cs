using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class Apprendre
    {
    
        // Clés Étrangères (Clé Composite Logique)
        [Required]
        public int AdherentId { get; set; }

        [Required]
        public int DisciplineId { get; set; }


        // Propriétés de Navigation (Relations EF Core)
        [ForeignKey(nameof(AdherentId))]
        public Adherent AdherentApprenant { get; set; } 

        [ForeignKey(nameof(DisciplineId))]
        public Discipline DisciplinePratiquee { get; set; } 
    }
}
