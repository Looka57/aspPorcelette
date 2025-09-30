using ASPPorcelette.API.DTOs.Compte;
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
    public class CompteService : ICompteService
    {
        private readonly ICompteRepository _compteRepository;
        private readonly IMapper _mapper;
        // NOTE: Nous devrons introduire l'ITransactionRepository plus tard pour des requêtes optimisées

        public CompteService(ICompteRepository compteRepository, IMapper mapper)
        {
            _compteRepository = compteRepository;
            _mapper = mapper;
        }

        // Méthode utilitaire pour calculer le solde actuel
        private CompteDto CalculateBalance(Compte compte)
        {
            // Mappe les propriétés de base (CompteId, Nom, Solde, Epargne)
            var compteDto = _mapper.Map<CompteDto>(compte);

            // Commence avec le solde initial (qui est maintenant "Solde" dans le modèle)
            decimal soldeActuel = compte.Solde; 
            
            // Correction : Utiliser compte.Transactions au lieu de compte.TransactionsEffectuees
            if (compte.Transactions != null)
            {
                soldeActuel += compte.Transactions.Sum(t => t.Montant);
            }

            compteDto.SoldeActuel = soldeActuel;
            return compteDto;
        }

        public async Task<IEnumerable<CompteDto>> GetAllWithBalanceAsync()
        {
            var comptes = await _compteRepository.GetAllAsync();
            
            // Calculer le solde pour chaque compte et mapper vers CompteDto
            var comptesDto = comptes.Select(CalculateBalance).ToList();
            
            return comptesDto;
        }

        public async Task<CompteDto?> GetByIdWithBalanceAsync(int id)
        {
            var compte = await _compteRepository.GetByIdAsync(id);
            if (compte == null)
            {
                return null;
            }
            // Calculer et retourner le DTO avec le solde
            return CalculateBalance(compte);
        }

        public async Task<Compte> CreateAsync(CompteCreateDto createDto)
        {
            var compteEntity = _mapper.Map<Compte>(createDto);
            return await _compteRepository.AddAsync(compteEntity);
        }

        public async Task<bool> UpdateAsync(int id, CompteUpdateDto updateDto)
        {
            var compteEntity = await _compteRepository.GetByIdAsync(id);
            if (compteEntity == null)
            {
                return false;
            }

            // Mappe le DTO sur l'entité existante
            _mapper.Map(updateDto, compteEntity);

            return await _compteRepository.UpdateAsync(compteEntity);
        }

        public async Task<(Compte? Compte, bool Success)> PartialUpdateAsync(
            int id, 
            JsonPatchDocument<CompteUpdateDto> patchDocument
        )
        {
            var compteEntity = await _compteRepository.GetByIdAsync(id);
            if (compteEntity == null)
            {
                return (null, false);
            }

            // 1. Mapper l'entité existante vers un DTO de mise à jour pour appliquer le patch
            var compteDtoToPatch = _mapper.Map<CompteUpdateDto>(compteEntity);
            patchDocument.ApplyTo(compteDtoToPatch);

            // 2. Mapper le DTO patché sur l'entité originale
            _mapper.Map(compteDtoToPatch, compteEntity);

            var success = await _compteRepository.UpdateAsync(compteEntity);
            
            // Recharger l'entité mise à jour avec les relations (pour le solde si besoin)
            var updatedCompte = await _compteRepository.GetByIdAsync(id);

            return (updatedCompte, success);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _compteRepository.DeleteAsync(id);
        }
    }
}
