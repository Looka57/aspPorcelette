using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.Models
{
    public class TypeEvenement
    {
        [Key]
        public int TypeEvenementId { get; set; }

        [Required]
        [MaxLength(50)]   
        public string Libelle { get; set; }

        // -----------------------------------------------------------------
        // Propriété de Navigation (Lien 1-à-N : Un type a plusieurs événements)
        // -----------------------------------------------------------------
        
        public ICollection<Evenement> EvenementsAssocies { get; set; } = new List<Evenement>();
    }
}