using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using AutoMapper;

namespace ASPPorcelette.API.Services
{
    public class AdherentService : IAdherentService
    {
        private readonly IAdherentRepository _adhrentRepository;
        private readonly IMapper _mapper;

        public AdherentService(IAdherentRepository adherentRepository, IMapper mapper)
        {
            _adhrentRepository = adherentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Adherent>> GetAllAsync()
        {
            return await _adhrentRepository.GetAllAsync();
        }

        public async Task<Adherent?> GetByIdAsync(int id)
        {
            return await _adhrentRepository.GetByIdAsync(id);
        }

        public async Task<Adherent> AddAsync(Adherent adherent)
        {
            return await _adhrentRepository.AddAsync(adherent);
        }

        public async Task<Adherent> UpdateAdherent(Adherent adherent)
        {
            return await _adhrentRepository.UpdateAdherent(adherent);
        }

        public async Task<(Adherent? Adherent, bool Success)> PartialUpdateAdherentAsync(
            int id,
            Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<AdherentUpdateDto> patchDocument
        )
        {
            // 1. Récupérer l'entité Adherent existante
            var adherentEntity = await _adhrentRepository.GetByIdAsync(id);
            if (adherentEntity == null)
            {
                return (null, false);
            }

            // 2. Mapper l'entité existante vers un DTO de mise à jour (Point de départ du patch)
            var adherentDtoToPatch = _mapper.Map<AdherentUpdateDto>(adherentEntity);

            // 3. Appliquer le patch au DTO
            patchDocument.ApplyTo(adherentDtoToPatch);

            // 4. Valider le DTO après application du patch (optionnel, mais recommandé)
            // Vous pouvez ajouter une validation ici si nécessaire

            // 5. Mapper le DTO mis à jour de retour vers l'entité
            _mapper.Map(adherentDtoToPatch, adherentEntity);

            // 6. Sauvegarder les changements dans la base de données
            var updatedAdherent = await _adhrentRepository.UpdateAdherent(adherentEntity);

            return (updatedAdherent, true);
        }

        public async Task<Adherent> DeleteAdherent(int id)
        {
            return await _adhrentRepository.DeleteAdherent(id);
        }

        public bool SaveChanges()
        {
            return _adhrentRepository.SaveChanges();
        }
    }
}