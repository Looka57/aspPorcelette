using ASPPorcelette.API.DTOs.Tarif;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface ITarifService
    {
        // CRUD READ
        Task<IEnumerable<Tarif>> GetAllTarifsAsync();
        Task<Tarif?> GetTarifByIdAsync(int id);
        
        // CRUD CREATE
        Task<Tarif> CreateTarifAsync(TarifCreateDto tarifDto);
        
        // CRUD UPDATE (PUT)
        Task<Tarif?> UpdateTarifAsync(int id, TarifUpdateDto tarifDto);

        // CRUD UPDATE (PATCH)
        Task<(Tarif? Tarif, bool Success)> PartialUpdateTarifAsync(
            int id, 
            JsonPatchDocument<TarifUpdateDto> patchDocument
        );

        // CRUD DELETE
        Task<bool> DeleteTarifAsync(int id);
    }
}
