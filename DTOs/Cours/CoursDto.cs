namespace ASPPorcelette.API.DTOs.Cours
{
    public class CoursDto
    {
        public int CoursId { get; set; }
        public string Libelle { get; set; } 
        
        // Inclut les DTOs des relations
        public DTOs.Discipline.DisciplineDto Discipline { get; set; } 
        public DTOs.Sensei.SenseiDto Sensei { get; set; } 

      
    }
}