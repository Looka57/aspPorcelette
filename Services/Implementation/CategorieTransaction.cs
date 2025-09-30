using ASPPorcelette.API.DTOs.CategorieTransaction;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class CategorieTransactionService : ICategorieTransactionService
    {
        private readonly ICategorieTransactionRepository _categorieRepository;
        private readonly IMapper _mapper;

        public CategorieTransactionService(ICategorieTransactionRepository categorieRepository, IMapper mapper)
        {
            _categorieRepository = categorieRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategorieTransaction>> GetAllAsync()
        {
            return await _categorieRepository.GetAllAsync();
        }

        public async Task<CategorieTransaction?> GetByIdAsync(int id)
        {
            return await _categorieRepository.GetByIdAsync(id);
        }

        public async Task<CategorieTransaction> CreateAsync(CategorieTransactionCreateDto createDto)
        {
            var categorie = _mapper.Map<CategorieTransaction>(createDto);
            return await _categorieRepository.AddAsync(categorie);
        }

        public async Task<bool> UpdateAsync(int id, CategorieTransactionUpdateDto updateDto)
        {
            var existingCategorie = await _categorieRepository.GetByIdAsync(id);
            if (existingCategorie == null)
            {
                return false;
            }

            // Mappe le DTO sur l'entité existante, puis met à jour dans la DB
            _mapper.Map(updateDto, existingCategorie);

            return await _categorieRepository.UpdateAsync(existingCategorie);
        }

        public async Task<(CategorieTransaction? Categorie, bool Success)> PartialUpdateAsync(
            int id, 
            JsonPatchDocument<CategorieTransactionUpdateDto> patchDocument
        )
        {
            var categorieEntity = await _categorieRepository.GetByIdAsync(id);
            if (categorieEntity == null)
            {
                return (null, false);
            }

            // Mappage de l'entité vers un DTO pour appliquer le patch
            var categorieDtoToPatch = _mapper.Map<CategorieTransactionUpdateDto>(categorieEntity);
            
            patchDocument.ApplyTo(categorieDtoToPatch);

            // Mappage du DTO patché vers l'entité
            _mapper.Map(categorieDtoToPatch, categorieEntity);

            var success = await _categorieRepository.UpdateAsync(categorieEntity);

            // Recharger l'entité complète (même si non nécessaire ici, bonne pratique)
            var updatedCategorie = await _categorieRepository.GetByIdAsync(id); 

            return (updatedCategorie, success);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _categorieRepository.DeleteAsync(id);
        }
    }
}
