using ASPPorcelette.API.Data;
using ASPPorcelette.API.DTOs.Actualite;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class ActualiteService : IActualiteService
    {
        private readonly IActualiteRepository _actualiteRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public ActualiteService(IActualiteRepository actualiteRepository, IMapper mapper, UserManager<User> userManager,  ApplicationDbContext context)
        {
            _actualiteRepository = actualiteRepository;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        // -----------------------------------------------------------------
        // Lecture (READ)
        // -----------------------------------------------------------------

        public async Task<IEnumerable<Actualite>> GetAllAsync()
        {
        return await _actualiteRepository.GetAllWithDetailsAsync();
        }

        public async Task<Actualite?> GetByIdAsync(int id)
        {
               return await _actualiteRepository.GetByIdWithDetailsAsync(id);
        }

        // -----------------------------------------------------------------
        // Création (CREATE)
        // -----------------------------------------------------------------

      public async Task<Actualite> CreateAsync(ActualiteCreateDto createDto)
{
    // On récupère l'utilisateur (Sensei) via le UserId
    var user = await _userManager.FindByIdAsync(createDto.UserId);
    if (user == null)
    {
        throw new Exception("Utilisateur introuvable");
    }

    var actualite = new Actualite
    {
        Titre = createDto.Titre,
        Contenu = createDto.Contenu,
        ImageUrl = createDto.ImageUrl,
        UserId = user.Id,
        DateDePublication = DateTime.UtcNow,
    };

    _context.Actualites.Add(actualite);
    await _context.SaveChangesAsync();

    return actualite;
}

        // -----------------------------------------------------------------
        // Mise à Jour (UPDATE - PUT)
        // -----------------------------------------------------------------

        public async Task<bool> UpdateAsync(int id, ActualiteUpdateDto updateDto)
        {
            // Mappe le DTO sur une nouvelle entité et s'assure que l'ID est correct
            var actualiteToUpdate = _mapper.Map<Actualite>(updateDto);
            actualiteToUpdate.ActualiteId = id;
            
            // Le repository gère la vérification d'existence et la mise à jour
            return await _actualiteRepository.UpdateAsync(actualiteToUpdate);
        }

        // -----------------------------------------------------------------
        // Mise à Jour Partielle (UPDATE - PATCH)
        // -----------------------------------------------------------------

        public async Task<(Actualite? Actualite, bool Success)> PartialUpdateAsync(
            int id,
            JsonPatchDocument<ActualiteUpdateDto> patchDocument
        )
        {
            var actualiteEntity = await _actualiteRepository.GetByIdAsync(id);
            if (actualiteEntity == null)
            {
                return (null, false);
            }

            // 1. Mappage de l'entité vers le DTO de mise à jour pour appliquer le patch
            var actualiteDtoToPatch = _mapper.Map<ActualiteUpdateDto>(actualiteEntity);
            patchDocument.ApplyTo(actualiteDtoToPatch);

            // 2. Mappage inverse du DTO patché vers l'entité existante
            _mapper.Map(actualiteDtoToPatch, actualiteEntity);
            
            // 3. Mise à jour dans la base de données
            var success = await _actualiteRepository.UpdateAsync(actualiteEntity);

            if (success)
            {
                // Recharger l'entité avec les relations pour le retour au client
                var updatedActualite = await _actualiteRepository.GetByIdWithDetailsAsync(id);
                return (updatedActualite, true);
            }
            
            return (null, false);
        }

        // -----------------------------------------------------------------
        // Suppression (DELETE)
        // -----------------------------------------------------------------

        public async Task<bool> DeleteAsync(int id)
        {
            return await _actualiteRepository.DeleteAsync(id);
        }
    }
}
