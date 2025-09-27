using AutoMapper;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs.Discipline;

namespace ASPPorcelette.API.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping pour Discipline (Modèle vers DTO)
            CreateMap<Discipline, DisciplineDto>();

            // Mapping pour Sensei (Modèle vers DTO)
            CreateMap<Sensei, SenseiDto>();
            CreateMap<SenseiCreateDto, Sensei>();//(POST : du DTO de création au Modèle)
            CreateMap<SenseiUpdateDto, Sensei>();//(PUT/PATCH : du DTO de mise à jour au Modèle existant)
            CreateMap<Sensei, SenseiUpdateDto>();//(PATCH : du Modèle existant au DTO, pour initialiser le patch)

            // Mapping pour Sensei (Modèle vers DTO)
            // AutoMapper gère automatiquement les propriétés ayant le même nom.
            // Il sait aussi mapper la propriété 'DisciplinePrincipale' (qui est un objet) 
            // vers le 'DisciplinePrincipale' (qui est un DTO), car nous avons défini un mapping pour Discipline -> DisciplineDto.
            CreateMap<SenseiCreateDto, Sensei>();
        }
    }
}
