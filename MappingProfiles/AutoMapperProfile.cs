using AutoMapper;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs.Discipline;
using ASPPorcelette.API.DTOs.Adherent;

namespace ASPPorcelette.API.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        // AutoMapper gère automatiquement les propriétés ayant le même nom.
        // Il sait aussi mapper la propriété 'DisciplinePrincipale' (qui est un objet) 
        // vers le 'DisciplinePrincipale' (qui est un DTO), car nous avons défini un mapping pour Discipline -> DisciplineDto.
        public AutoMapperProfile()
        {
            // ----------------------------------------------------
            // Mapping pour Discipline (Modèle vers DTO)     
            CreateMap<Discipline, DisciplineDto>();
            // ----------------------------------------------------

            // ----------------------------------------------------
            // Mapping pour Sensei (Modèle vers DTO)
            // ----------------------------------------------------
            CreateMap<Sensei, SenseiDto>();
            CreateMap<SenseiCreateDto, Sensei>();//(POST : du DTO de création au Modèle)
            CreateMap<SenseiUpdateDto, Sensei>();//(PUT/PATCH : du DTO de mise à jour au Modèle existant)
            CreateMap<Sensei, SenseiUpdateDto>();//(PATCH : du Modèle existant au DTO, pour initialiser le patch)


            // ----------------------------------------------------
            // Mappings pour la ressource Adherent (CRUD complet)
            // ----------------------------------------------------
            // 1. Entité vers DTO de sortie (CORRIGÉ : MANQUANT PRÉCÉDEMMENT)
            CreateMap<Adherent, AdherentDto>();

            // 2. Entité vers DTO de mise à jour (pour l'initialisation du PATCH)
            CreateMap<Adherent, AdherentUpdateDto>();

            // 3. DTO de création vers Entité (POST)
            CreateMap<AdherentCreateDto, Adherent>();

            // 4. DTO de mise à jour vers Entité (PUT/PATCH)
            CreateMap<AdherentUpdateDto, Adherent>();
        }
    }
}
