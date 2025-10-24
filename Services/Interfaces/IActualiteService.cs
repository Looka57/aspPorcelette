using ASPPorcelette.API.DTOs.Actualite;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface IActualiteService
    {
        // CRUD
        Task<IEnumerable<Actualite>> GetAllAsync();
        Task<Actualite?> GetByIdAsync(int id);
        Task<Actualite> CreateAsync(ActualiteCreateDto createDto);
        Task<bool> UpdateAsync(int id, ActualiteUpdateDto updateDto, string webRootPath);
        Task<(Actualite? Actualite, bool Success)> PartialUpdateAsync(
            int id, 
            JsonPatchDocument<ActualiteUpdateDto> patchDocument
        );
        Task<bool> DeleteAsync(int id, string webRootPath);
    }
}
