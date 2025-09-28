using ASPPorcelette.API.DTOs.Apprendre;
using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface IApprendreService
    {
        // GET All: Récupère toutes les inscriptions avec leurs relations (Adhérent & Discipline)
        Task<IEnumerable<Apprendre>> GetAllInscriptionsAsync();

        Task<Apprendre?> GetInscriptionByIdsAsync(int adherentId, int disciplineId); 

        
        // GET By ID: Récupère une inscription spécifique avec ses relations
        Task<Apprendre?> GetInscriptionByIdAsync(int id);
        
        // POST: Crée une nouvelle inscription
        Task<Apprendre> CreateInscriptionAsync(ApprendreCreateDto inscriptionDto);

        // DELETE: Supprime une inscription
        Task<bool> DeleteInscriptionAsync(int adherentId, int disciplineId); 
    }
}
