using ASPPorcelette.API.DTOs.Cours;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class CoursService : ICoursService
    {
        private readonly ICoursRepository _coursRepository;
        private readonly IMapper _mapper;

        public CoursService(ICoursRepository coursRepository, IMapper mapper)
        {
            _coursRepository = coursRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Cours>> GetAllCoursWithDetailsAsync()
    {
        return await _coursRepository.GetAllCoursWithDetailsAsync();
    }
    
    public async Task<Cours?> GetCoursWithDetailsAsync(int id)
    {
        return await _coursRepository.GetCoursWithDetailsAsync(id);
    }

        public async Task<IEnumerable<Cours>> GetAllAsync()
        {
            // Utilise la méthode qui charge les relations pour l'affichage
            return await _coursRepository.GetAllCoursWithDetailsAsync();
        }

        public async Task<Cours?> GetByIdAsync(int id)
        {
            // Utilise la méthode qui charge les relations pour l'affichage
            return await _coursRepository.GetCoursWithDetailsAsync(id);
        }

        public async Task<Cours> AddAsync(Cours cours)
        {
            // Logique métier avant l'ajout si nécessaire
            return await _coursRepository.AddAsync(cours);
        }

        public async Task<Cours> UpdateCours(Cours cours)
        {
            // Logique métier avant la mise à jour si nécessaire
            return await _coursRepository.UpdateCours(cours);
        }
        
        // Implémentation du PATCH, identique à Adherent
        public async Task<(Cours? Cours, bool Success)> PartialUpdateCoursAsync(
            int id,
            JsonPatchDocument<CoursUpdateDto> patchDocument
        )
        {
            var coursEntity = await _coursRepository.GetByIdAsync(id);
            if (coursEntity == null)
            {
                return (null, false);
            }

            var coursDtoToPatch = _mapper.Map<CoursUpdateDto>(coursEntity);
            patchDocument.ApplyTo(coursDtoToPatch);

            _mapper.Map(coursDtoToPatch, coursEntity);

            var updatedCours = await _coursRepository.UpdateCours(coursEntity);

            // Recharger les détails après la mise à jour (si les IDs ont changé)
            // Bien que dans ce cas, nous n'avons pas besoin de recharger toutes les relations,
            // on appelle GetByIdAsync qui utilisera le FindAsync rapide.
            // Si vous voulez le DTO complet, vous pouvez appeler GetCoursWithDetailsAsync(id) ici.
            var coursWithDetails = await _coursRepository.GetCoursWithDetailsAsync(id);

            return (coursWithDetails, true);
        }

        public async Task<Cours> DeleteCours(int id)
        {
            // Logique métier avant la suppression si nécessaire
            return await _coursRepository.DeleteCours(id);
        }

        public bool SaveChanges()
        {
            return _coursRepository.SaveChanges();
        }
    }
}
