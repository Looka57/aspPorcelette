using AutoMapper;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Sensei;

namespace ASPPorcelette.API.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping pour Discipline (Modèle vers DTO)
            CreateMap<Discipline, DisciplineDto>();
            CreateMap<Sensei, SenseiDto>();

            // Mapping pour Sensei (Modèle vers DTO)
            // AutoMapper gère automatiquement les propriétés ayant le même nom.
            // Il sait aussi mapper la propriété 'DisciplinePrincipale' (qui est un objet) 
            // vers le 'DisciplinePrincipale' (qui est un DTO), car nous avons défini un mapping pour Discipline -> DisciplineDto.
            CreateMap<SenseiCreateDto, Sensei>();
        }
    }
}
