// Fichier : Services/Implementation/TypeEvenementService.cs
using ASPPorcelette.API.DTOs.TypeEvenement;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class TypeEvenementService : ITypeEvenementService
    {
        private readonly ITypeEvenementRepository _repository;
        private readonly IMapper _mapper;

        public TypeEvenementService(ITypeEvenementRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TypeEvenementDto>> GetAllTypesAsync()
        {
            var types = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TypeEvenementDto>>(types);
        }

        public async Task<TypeEvenementDto?> GetTypeByIdAsync(int id)
        {
            var typeEvenement = await _repository.GetByIdAsync(id);
            if (typeEvenement == null)
            {
                return null;
            }
            return _mapper.Map<TypeEvenementDto>(typeEvenement);
        }

        public async Task<TypeEvenementDto> CreateTypeAsync(TypeEvenementCreateDto createDto)
        {
            var typeEvenementEntity = _mapper.Map<TypeEvenement>(createDto);
            var createdEntity = await _repository.AddAsync(typeEvenementEntity);
            return _mapper.Map<TypeEvenementDto>(createdEntity);
        }

        // CORRIGÉ : Implémentation utilisant Task<bool> et TypeEvenementUpdateDto
       public async Task<bool> UpdateTypeAsync(int id, TypeEvenementUpdateDto updateDto)
{
    var typeToUpdate = _mapper.Map<TypeEvenement>(updateDto);
    typeToUpdate.TypeEvenementId = id;

    var updatedEntity = await _repository.UpdateAsync(typeToUpdate);

    return updatedEntity != null; // True si mise à jour, false sinon
}

        
        public async Task<bool> DeleteTypeAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
