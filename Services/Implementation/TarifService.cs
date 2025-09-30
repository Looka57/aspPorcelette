using ASPPorcelette.API.DTOs.Tarif;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    // NOTE: Ce service utilise maintenant le ITarifRepository spécifique
    // pour garantir le chargement de la Discipline dans les méthodes GetAll et GetById.
    public class TarifService : ITarifService
    {
        // Changement: Utilisation du Repository spécifique pour le Tarif
        private readonly ITarifRepository _tarifRepository;
        private readonly IMapper _mapper;
        
        // Injection du service de discipline pour la validation de la FK
        private readonly IDisciplineService _disciplineService; 

        // NOTE: Le constructeur utilise désormais ITarifRepository
        public TarifService(
            ITarifRepository tarifRepository, // Changé de IGenericRepository<Tarif>
            IMapper mapper, 
            IDisciplineService disciplineService)
        {
            _tarifRepository = tarifRepository;
            _mapper = mapper;
            _disciplineService = disciplineService;
        }

        // --- VALIDATION HELPER ---
        private async Task<bool> IsDisciplineValid(int disciplineId)
        {
            // On appelle le service de Discipline pour vérifier l'existence
            return await _disciplineService.GetByIdAsync(disciplineId) != null;
        }

        // --- CRUD READ ---

        public async Task<IEnumerable<Tarif>> GetAllTarifsAsync()
        {
            // Utilisation de la méthode spécifique du Repository pour charger la Discipline
            return await _tarifRepository.GetAllTarifsWithDisciplineAsync();
        }

        public async Task<Tarif?> GetTarifByIdAsync(int id)
        {
            // Utilisation de la méthode spécifique du Repository pour charger la Discipline
            return await _tarifRepository.GetTarifByIdWithDisciplineAsync(id);
        }

        // --- CRUD CREATE ---

        public async Task<Tarif> CreateTarifAsync(TarifCreateDto tarifDto)
        {
            // 1. Validation de l'existence de la Discipline
            if (!await IsDisciplineValid(tarifDto.DisciplineId))
            {
                throw new KeyNotFoundException($"La Discipline avec ID {tarifDto.DisciplineId} est introuvable.");
            }

            // 2. Mapping et Ajout
            var tarifEntity = _mapper.Map<Tarif>(tarifDto);
            
            var createdTarif = await _tarifRepository.AddAsync(tarifEntity);
            
            // Recharger l'entité avec sa relation Discipline pour le retour 
            return await GetTarifByIdAsync(createdTarif.TarifId) ?? createdTarif; 
        }

        // --- CRUD UPDATE (PUT) ---

        public async Task<Tarif?> UpdateTarifAsync(int id, TarifUpdateDto tarifDto)
        {
            // On charge l'entité avec la relation Discipline (pour le retour)
            var existingTarif = await _tarifRepository.GetTarifByIdWithDisciplineAsync(id);
            if (existingTarif == null)
            {
                return null;
            }

            // 1. Validation de la nouvelle Discipline (si elle est fournie)
            if (tarifDto.DisciplineId.HasValue && !await IsDisciplineValid(tarifDto.DisciplineId.Value))
            {
                throw new KeyNotFoundException($"La nouvelle Discipline avec ID {tarifDto.DisciplineId.Value} est introuvable.");
            }

            // 2. Mapping du DTO sur l'entité existante
            _mapper.Map(tarifDto, existingTarif);

            // 3. Mise à jour dans le Repository
            await _tarifRepository.UpdateAsync(existingTarif);
            
            // On retourne l'entité mise à jour avec les relations chargées
            return existingTarif;
        }

        // --- CRUD UPDATE (PATCH) ---
        
        public async Task<(Tarif? Tarif, bool Success)> PartialUpdateTarifAsync(
            int id, 
            JsonPatchDocument<TarifUpdateDto> patchDocument)
        {
            // Utilisation de la méthode avec relations pour obtenir l'objet complet si nécessaire
            var tarifEntity = await _tarifRepository.GetTarifByIdWithDisciplineAsync(id);
            if (tarifEntity == null)
            {
                return (null, false);
            }

            // 1. Mapping de l'entité sur le DTO pour l'application du patch
            var tarifDtoToPatch = _mapper.Map<TarifUpdateDto>(tarifEntity);
            patchDocument.ApplyTo(tarifDtoToPatch);

            // 2. Validation si la Discipline a été modifiée via le patch
            if (tarifDtoToPatch.DisciplineId.HasValue && !await IsDisciplineValid(tarifDtoToPatch.DisciplineId.Value))
            {
                // Si la validation échoue après le patch, on retourne l'échec
                return (null, false);
            }

            // 3. Re-mapping du DTO patché sur l'entité existante
            _mapper.Map(tarifDtoToPatch, tarifEntity);

            // 4. Mise à jour et retour
            await _tarifRepository.UpdateAsync(tarifEntity);
            
            // On retourne l'entité mise à jour avec les relations chargées
            return (tarifEntity, true);
        }

        // --- CRUD DELETE ---

        public async Task<bool> DeleteTarifAsync(int id)
        {
            // CORRECTION: Appelle la méthode DeleteAsync du Repository qui prend l'ID (int)
            return await _tarifRepository.DeleteAsync(id); 
        }
    }
}
