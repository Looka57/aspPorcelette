using ASPPorcelette.API.Data;
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPPorcelette.API.Services
{
    public class SaisonStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public SaisonStatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gèle les statistiques de la saison précédente.
        /// Le gel est effectué à partir du 1er juillet.
        /// </summary>
        public async Task FreezePreviousSeasonAsync()
        {
            var today = DateTime.Today;

            // Avant le 1er juillet, on ne fait rien.
            if (today.Month < 7)
                return;

            // Exemple :
            // aujourd'hui = 01/07/2026
            // saison terminée = 2025-2026
            var endYear = today.Year;
            var startYear = endYear - 1;
            var saison = $"{startYear}-{endYear}";

            // Si la saison est déjà gelée, on ne recommence pas.
            var alreadyFrozen = await _context.StatistiquesSaisons
                .AnyAsync(s => s.Saison == saison);

            if (alreadyFrozen)
                return;

            // Période de la saison :
            // 01/09/2025 → 30/06/2026
            var dateDebut = new DateTime(startYear, 9, 1);
            var dateFin = new DateTime(endYear, 6, 30, 23, 59, 59);

            // On récupère directement les utilisateurs ayant une
            // date d'adhésion pendant cette saison et une discipline.
            var utilisateurs = await _context.Users
                .Where(u =>
                    u.DateAdhesion >= dateDebut &&
                    u.DateAdhesion <= dateFin &&
                    u.DisciplineId.HasValue)
                .ToListAsync();

            // On groupe les adhérents par discipline.
            var statistiques = utilisateurs
                .GroupBy(u => u.DisciplineId!.Value)
                .Select(g => new StatistiqueSaison
                {
                    Saison = saison,
                    DisciplineId = g.Key,
                    TotalInscrits = g.Count()
                })
                .ToList();

            // Aucun adhérent trouvé.
            if (statistiques.Count == 0)
                return;

            // Enregistrement définitif des statistiques.
            await _context.StatistiquesSaisons.AddRangeAsync(statistiques);
            await _context.SaveChangesAsync();
        }

        public async Task<List<object>> GetStatisticsBySeasonAsync(string saison)
        {
            return await _context.StatistiquesSaisons
                .Include(s => s.Discipline)
                .Where(s => s.Saison == saison)
                .OrderByDescending(s => s.TotalInscrits)
                .Select(s => new
                {
                    DisciplineId = s.DisciplineId,
                    Discipline = s.Discipline.Nom,
                    TotalInscrits = s.TotalInscrits
                })
                .Cast<object>()
                .ToListAsync();
        }
    }
}