using ASPPorcelette.API.Data;
using ASPPorcelette.API.DTOs.Evenement;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class EvenementService : IEvenementService
    {
        private readonly ApplicationDbContext _context; 
        private readonly IEvenementRepository _repository;
        private readonly IMapper _mapper;

        public EvenementService(IEvenementRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // -----------------------------------------------------------------
        // READ
        // -----------------------------------------------------------------

        public async Task<IEnumerable<Evenement>> GetAllEvenementsAsync()
        {
            // Utilise la méthode du repository qui charge les détails pour le DTO de réponse
            return await _repository.GetAllEvenementsWithDetailsAsync();
        }

        public async Task<Evenement?> GetEvenementByIdAsync(int id)
        {
            // Utilise la bonne méthode du repository pour charger les détails
            return await _repository.GetEvenementWithDetailsAsync(id);
        }

        // -----------------------------------------------------------------
        // CREATE
        // -----------------------------------------------------------------

        public async Task<Evenement> CreateEvenementAsync(EvenementCreateDto createDto)
        {
            // Mappage du DTO vers l'entité
            var evenementEntity = _mapper.Map<Evenement>(createDto);

            // Logique métier avant l'ajout si nécessaire (ex: validation de la date)

            // Ajout au repository
            return await _repository.AddAsync(evenementEntity);
        }

        // -----------------------------------------------------------------
        // UPDATE (PUT)
        // -----------------------------------------------------------------

        public async Task<bool> UpdateEvenementAsync(int id, EvenementUpdateDto updateDto)
        {
            // 1. Charger l'entité existante (nécessaire si on voulait faire des vérifications)
            // Cependant, le repository gère le 'find' dans son Update, donc nous mappons directement.

            // 2. Mappage du DTO vers l'entité (l'ID sera ajouté ci-dessous)
            var evenementToUpdate = _mapper.Map<Evenement>(updateDto);
            evenementToUpdate.EvenementId = id;

            // 3. Mise à jour
            return await _repository.UpdateAsync(evenementToUpdate);
        }

        // -----------------------------------------------------------------
        // UPDATE (PATCH)
        // -----------------------------------------------------------------

        public async Task<(Evenement? Evenement, bool Success)> PartialUpdateEvenementAsync(
            int id,
            JsonPatchDocument<EvenementUpdateDto> patchDocument
        )
        {
            // 1. Récupérer l'entité existante (sans les détails)
            var evenementEntity = await _repository.GetByIdAsync(id);
            if (evenementEntity == null)
            {
                return (null, false);
            }

            // 2. Mappage de l'entité vers le DTO de mise à jour (pour l'application du patch)
            var evenementDtoToPatch = _mapper.Map<EvenementUpdateDto>(evenementEntity);

            // 3. Appliquer les opérations de patch
            patchDocument.ApplyTo(evenementDtoToPatch);

            // 4. Mappage du DTO patché vers l'entité existante
            _mapper.Map(evenementDtoToPatch, evenementEntity);

            // 5. Mettre à jour dans la base (retourne true/false)
            var success = await _repository.UpdateAsync(evenementEntity);

            if (!success)
            {
                return (null, false);
            }

            // 6. Recharger l'événement avec les relations Sensei et Discipline pour le retour
            var evenementWithDetails = await _repository.GetEvenementWithDetailsAsync(id);

            return (evenementWithDetails, true);
        }

        // -----------------------------------------------------------------
        // DELETE
        // -----------------------------------------------------------------

        public async Task<bool> DeleteEvenementAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
