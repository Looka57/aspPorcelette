using ASPPorcelette.API.DTOs.Evenement;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface IEvenementService
    {
        // CRUD READ
        // Utilise les méthodes qui chargent les détails pour le DTO de sortie
        Task<IEnumerable<Evenement>> GetAllEvenementsAsync(); 
        Task<Evenement?> GetEvenementByIdAsync(int id);
        
        // CRUD CREATE
        Task<Evenement> CreateEvenementAsync(EvenementCreateDto createDto);
        
        // CRUD UPDATE (PUT)
        Task<bool> UpdateEvenementAsync(int id, EvenementUpdateDto updateDto);
        
        // CRUD UPDATE (PATCH)
        Task<(Evenement? Evenement, bool Success)> PartialUpdateEvenementAsync(
            int id, 
            JsonPatchDocument<EvenementUpdateDto> patchDocument
        );
        
        // CRUD DELETE
        Task<bool> DeleteEvenementAsync(int id);
    }
}
