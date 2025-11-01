using ASPPorcelette.API.DTOs.Horaire;

namespace ASPPorcelette.API.DTOs.Cours
{
    public class CoursDto
    {
        public int CoursId { get; set; }
        public string Libelle { get; set; } 

        public UserDto Sensei { get; set; } // optionnel si tu veux afficher le nom
        
        // Inclut les DTOs des relations
        public DTOs.Discipline.DisciplineDto Discipline { get; set; } 
        public string UserId { get; set; }

         public ICollection<HoraireDto> Horaires { get; set; } = new List<HoraireDto>();


      
    }
}