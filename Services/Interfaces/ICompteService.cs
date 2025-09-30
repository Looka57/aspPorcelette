using ASPPorcelette.API.DTOs.Compte;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface ICompteService
    {
        // Retourne les comptes AVEC le solde calculé (qui inclut le solde actuel)
        Task<IEnumerable<CompteDto>> GetAllWithBalanceAsync();
        Task<CompteDto?> GetByIdWithBalanceAsync(int id);
        Task<Compte> CreateAsync(CompteCreateDto createDto);
        Task<bool> UpdateAsync(int id, CompteUpdateDto updateDto);
        Task<(Compte? Compte, bool Success)> PartialUpdateAsync(
            int id, 
            JsonPatchDocument<CompteUpdateDto> patchDocument
        );
        Task<bool> DeleteAsync(int id);
    }
}
