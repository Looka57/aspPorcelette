// Fichier : Services/Interfaces/IHoraireService.cs
using ASPPorcelette.API.DTOs.Horaire;
using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface IHoraireService
    {
        Task<IEnumerable<HoraireDto>> GetAllHorairesAsync();
        Task<HoraireDto?> GetHoraireByIdAsync(int id);
        // Task<HoraireDto> CreateHoraireAsync(HoraireCreateDto horaireDto);
        // Task<bool> UpdateHoraireAsync(int id, HoraireCreateDto horaireDto);
        // Task<bool> DeleteHoraireAsync(int id);
    }
}