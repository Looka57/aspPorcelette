using ASPPorcelette.API.DTOs.Cours;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface ICoursService
    {
        // Utilise les méthodes avec détails pour la sortie
        Task<IEnumerable<Cours>> GetAllAsync(); 
        Task<Cours?> GetByIdAsync(int id);
        
        Task<Cours> AddAsync(Cours cours);
        Task<Cours> UpdateCours(Cours cours);
        Task<(Cours? Cours, bool Success)> PartialUpdateCoursAsync(
            int id, 
            JsonPatchDocument<CoursUpdateDto> patchDocument
        );
        Task<Cours> DeleteCours(int id);
        bool SaveChanges();
    }
}
