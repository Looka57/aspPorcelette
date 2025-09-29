// Fichier : Repository/Interfaces/ITypeEvenementRepository.cs
using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface ITypeEvenementRepository
    {
        Task<IEnumerable<TypeEvenement>> GetAllAsync();
        Task<TypeEvenement?> GetByIdAsync(int id);
        Task<TypeEvenement> AddAsync(TypeEvenement typeEvenement);
        Task<TypeEvenement?> UpdateAsync(TypeEvenement typeEvenement);
        Task<bool> DeleteAsync(int id);
    }
}