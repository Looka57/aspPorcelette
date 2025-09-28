using ASPPorcelette.API.Models;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface IAdherentRepository
    {
        // Méthodes pour gérer les Adhérents
        Task<IEnumerable<Adherent>> GetAllAsync(); // Récupère tous les Adhérents
        Task<Adherent?> GetByIdAsync(int id); // Récupère un Adhérent par son ID
        Task<Adherent> AddAsync(Adherent adherent); // Ajoute un nouveau Adhérent
        Task<Adherent> UpdateAdherent(Adherent adherent);// Modifie un Adhérent
        Task<Adherent> DeleteAdherent(int id);// Supprime un Adhérent

        // Méthode pour sauvegarder les changements
        bool SaveChanges();
    }
}