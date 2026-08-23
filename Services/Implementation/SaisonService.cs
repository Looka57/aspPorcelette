using ASPPorcelette.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ASPPorcelette.API.Services
{
    public class SaisonService
    {
        private readonly ApplicationDbContext _context;

        public SaisonService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Détermine la saison correspondant à une date.
        /// Une saison va du 1er septembre au 30 juin.
        /// </summary>
        public string GetSeason(DateTime date)
        {
            // Septembre à décembre
            if (date.Month >= 9)
            {
                return $"{date.Year}-{date.Year + 1}";
            }

            // Janvier à août
            return $"{date.Year - 1}-{date.Year}";
        }

        /// <summary>
        /// Retourne la saison correspondant à aujourd'hui.
        /// </summary>
        public string GetCurrentSeason()
        {
            return GetSeason(DateTime.Today);
        }

        /// <summary>
        /// Retourne la saison qui doit être gelée
        /// à partir du 1er juillet.
        /// </summary>
        public string? GetSeasonToFreeze()
        {
            var today = DateTime.Today;

            // De septembre à juin :
            // aucune saison n'est encore à geler.
            if (today.Month >= 9 || today.Month <= 6)
            {
                return null;
            }

            // Juillet ou août :
            // la saison précédente doit être gelée.
            return $"{today.Year - 1}-{today.Year}";
        }

        /// <summary>
        /// Vérifie si une saison est déjà présente
        /// dans l'historique.
        /// </summary>
        public async Task<bool> IsSeasonAlreadyFrozenAsync(string saison)
        {
            return await _context.StatistiquesSaisons
                .AnyAsync(s => s.Saison == saison);
        }
    }
}