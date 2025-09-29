// Fichier : Services/Interfaces/ITypeEvenementService.cs
using ASPPorcelette.API.DTOs.TypeEvenement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface ITypeEvenementService
    {
        Task<IEnumerable<TypeEvenementDto>> GetAllTypesAsync();
        Task<TypeEvenementDto?> GetTypeByIdAsync(int id);
        Task<TypeEvenementDto> CreateTypeAsync(TypeEvenementCreateDto createDto);
        Task<bool> UpdateTypeAsync(int id, TypeEvenementUpdateDto updateDto);
        Task<bool> DeleteTypeAsync(int id);
    }
}