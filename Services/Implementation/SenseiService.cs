using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Sensei; // Assurez-vous d'avoir ce using
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch; // NOUVEAU
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services
{
    public class SenseiService : ISenseiService
    {
        private readonly ISenseiRepository _senseiRepository;
        private readonly IMapper _mapper;

        public SenseiService(ISenseiRepository senseiRepository, IMapper mapper)
        {
            _senseiRepository = senseiRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Sensei>> GetAllSenseisAsync()
        {
            return await _senseiRepository.GetAllAsync();
        }

        public async Task<Sensei?> GetSenseiByIdAsync(int id)
        {
            return await _senseiRepository.GetByIdAsync(id);
        }

        public async Task<Sensei> CreateSenseiAsync(Sensei sensei)
        {
            return await _senseiRepository.AddAsync(sensei);
        }

        public async Task<Sensei> UpdateSenseiAsync(Sensei sensei)
        {
            return await _senseiRepository.UpdateSensei(sensei);
        }

        // -----------------------------------------------------------------
        // NOUVEAU : Méthode PATCH pour la mise à jour partielle
        // -----------------------------------------------------------------
        public async Task<(Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(
            int id,
            JsonPatchDocument<SenseiUpdateDto> patchDocument
        )
        {
            // 1. Récupérer l'entité Sensei existante
            var senseiEntity = await _senseiRepository.GetByIdAsync(id);
            if (senseiEntity == null)
            {
                return (null, false);
            }

            // 2. Mapper l'entité existante vers un DTO de mise à jour (Point de départ du patch)
            var senseiDtoToPatch = _mapper.Map<SenseiUpdateDto>(senseiEntity);
            
            // 3. Appliquer le patch au DTO
            patchDocument.ApplyTo(senseiDtoToPatch);

            // 4. Mapper le DTO patché DANS l'entité existante (pour que EF Core suive les changements)
            _mapper.Map(senseiDtoToPatch, senseiEntity);

            // CORRECTION : L'ID doit être SenseiId
            senseiEntity.SenseiId = id;

            // 5. Sauvegarder les modifications via le Repository (Utilisation de la méthode de mise à jour standard)
            // J'utilise UpdateSensei() car c'est la méthode de mise à jour complète que vous aviez.
            await _senseiRepository.UpdateSensei(senseiEntity);
            
            // 6. Retourner le Sensei mis à jour
            return (senseiEntity, true);
        }

        public async Task<Sensei> DeleteSenseiAsync(int id)
        {
            // Si la méthode dans le repository retourne la Sensei supprimée
            return await _senseiRepository.DeleteSensei(id);
        }
    }
}
