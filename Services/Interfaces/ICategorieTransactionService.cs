using ASPPorcelette.API.DTOs.CategorieTransaction;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface ICategorieTransactionService
    {
        Task<IEnumerable<CategorieTransaction>> GetAllAsync();
        Task<CategorieTransaction?> GetByIdAsync(int id);
        Task<CategorieTransaction> CreateAsync(CategorieTransactionCreateDto createDto);
        Task<bool> UpdateAsync(int id, CategorieTransactionUpdateDto updateDto);
        
        Task<(CategorieTransaction? Categorie, bool Success)> PartialUpdateAsync(
            int id, 
            JsonPatchDocument<CategorieTransactionUpdateDto> patchDocument
        );
        Task<bool> DeleteAsync(int id);
    }
}
