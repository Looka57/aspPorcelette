// Fichier : Services/Implementation/HoraireService.cs
using ASPPorcelette.API.DTOs.Horaire;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class HoraireService : IHoraireService
    {
        private readonly IHoraireRepository _horaireRepository;
        private readonly IMapper _mapper;

        public HoraireService(IHoraireRepository horaireRepository, IMapper mapper)
        {
            _horaireRepository = horaireRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HoraireDto>> GetAllHorairesAsync()
        {
            // Récupère les horaires avec les relations (Cours)
            var horaires = await _horaireRepository.GetAllWithCoursAsync();
            
            // Mappe la liste des modèles vers la liste des DTOs
            return _mapper.Map<IEnumerable<HoraireDto>>(horaires);
        }

        public async Task<HoraireDto?> GetHoraireByIdAsync(int id)
        {
            // Récupère l'horaire avec les relations (Cours)
            var horaire = await _horaireRepository.GetByIdWithCoursAsync(id);
            if (horaire == null)
            {
                return null;
            }
            
            // Mappe le modèle vers le DTO
            return _mapper.Map<HoraireDto>(horaire);
        }

        // public async Task<HoraireDto> CreateHoraireAsync(HoraireCreateDto horaireDto)
        // {
        //     // 1. Mapping du DTO de création vers l'entité Modèle
        //     var horaireEntity = _mapper.Map<Horaire>(horaireDto);

        //     // 2. Ajout et sauvegarde via le Repository
        //     var createdHoraire = await _horaireRepository.AddAsync(horaireEntity);

        //     // 3. Mappage de l'entité créée vers le DTO de lecture pour le retour
        //     return _mapper.Map<HoraireDto>(createdHoraire);
        // }

        // public async Task<bool> UpdateHoraireAsync(int id, HoraireCreateDto horaireDto)
        // {
        //     // 1. Mapping du DTO vers l'entité Modèle
        //     var horaireToUpdate = _mapper.Map<Horaire>(horaireDto);
        //     horaireToUpdate.HoraireId = id; // Assurez-vous que l'ID est défini

        //     // 2. Mise à jour via le Repository
        //     var updatedHoraire = await _horaireRepository.UpdateAsync(horaireToUpdate);

        //     // Retourne true si l'entité a été trouvée et mise à jour
        //     return updatedHoraire != null;
        // }

        // public async Task<bool> DeleteHoraireAsync(int id)
        // {
        //     // Délègue la suppression au Repository
        //     return await _horaireRepository.DeleteAsync(id);
        // }
    }
}