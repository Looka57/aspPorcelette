using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface IAdherentService
    {
        // Méthodes pour gérer les Adhérents
        Task<IEnumerable<Adherent>> GetAllAsync(); // Récupère tous les Adhérents
        Task<Adherent?> GetByIdAsync(int id); // Récupère un Adhérent par son ID
        Task<Adherent> AddAsync(Adherent adherent); // Ajoute un nouveau Adhérent
        Task<Adherent> UpdateAdherent(Adherent adherent);// Modifie un Adhérent
         Task<(Adherent? Adherent, bool Success)> PartialUpdateAdherentAsync(
            int id, 
            JsonPatchDocument<AdherentUpdateDto> patchDocument
        );
        Task<Adherent> DeleteAdherent(int id);// Supprime un Adhérent

        // Méthode pour sauvegarder les changements
        bool SaveChanges();
    }
}