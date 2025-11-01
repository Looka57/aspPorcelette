using AutoMapper;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.DTOs;
// using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs.Discipline;
using ASPPorcelette.API.DTOs.Adherent;
using ASPPorcelette.API.DTOs.Evenement;
using ASPPorcelette.API.DTOs.Actualite;
using ASPPorcelette.API.DTOs.CategorieTransaction;
using ASPPorcelette.API.DTOs.Compte;
using ASPPorcelette.API.DTOs.Transaction;
using ASPPorcelette.API.DTOs.Tarif;
using ASPPorcelette.API.Models.Identity;

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
            // CreateMap<Sensei, SenseiDto>();
            // CreateMap<SenseiCreateDto, Sensei>();//(POST : du DTO de création au Modèle)
            // CreateMap<SenseiUpdateDto, Sensei>();//(PUT/PATCH : du DTO de mise à jour au Modèle existant)
            // CreateMap<Sensei, SenseiUpdateDto>();//(PATCH : du Modèle existant au DTO, pour initialiser le patch)

            // ----------------------------------------------------
            // Mappings pour la ressource Adherent (CRUD complet)
            // ----------------------------------------------------
            CreateMap<Adherent, AdherentDto>(); // 1. Entité vers DTO de sortie 
            CreateMap<Adherent, AdherentUpdateDto>();  // 2. Entité vers DTO de mise à jour (pour l'initialisation du PATCH)
            CreateMap<AdherentCreateDto, Adherent>();  // 3. DTO de création vers Entité (POST)
            CreateMap<AdherentUpdateDto, Adherent>();// 4. DTO de mise à jour vers Entité (PUT/PATCH)

            // ----------------------------------------------------
            // Mappings pour la ressource Cours
            // ----------------------------------------------------
            CreateMap<Models.Cours, DTOs.Cours.CoursDto>();   // 1. Entité vers DTO de sortie (GET)
            CreateMap<DTOs.Cours.CoursCreateDto, Models.Cours>();   // 2. DTO de création vers Entité (POST)
            CreateMap<Models.Cours, DTOs.Cours.CoursUpdateDto>();   // 3. Entité vers DTO de mise à jour (pour l'initialisation du PATCH)
            CreateMap<DTOs.Cours.CoursUpdateDto, Models.Cours>();  // 4. DTO de mise à jour vers Entité (PUT/PATCH)

            // / ----------------------------------------------------
            // Mappings pour la ressource Horaire
            // ----------------------------------------------------
            CreateMap<Models.Horaire, DTOs.Horaire.HoraireDto>();

            // / ----------------------------------------------------
            // Mappings pour la ressource Apprendre
            // ----------------------------------------------------
            CreateMap<Models.Apprendre, DTOs.Apprendre.ApprendreDto>();
            CreateMap<DTOs.Apprendre.ApprendreCreateDto, Models.Apprendre>();

            // / ----------------------------------------------------
            // Mappings pour la ressource TypeEvenement
            // ----------------------------------------------------
            CreateMap<TypeEvenement, DTOs.TypeEvenement.TypeEvenementDto>();
            CreateMap<DTOs.TypeEvenement.TypeEvenementCreateDto, TypeEvenement>();
            CreateMap<DTOs.TypeEvenement.TypeEvenementUpdateDto, TypeEvenement>();
            CreateMap<TypeEvenement, DTOs.TypeEvenement.TypeEvenementUpdateDto>();

            // / ----------------------------------------------------
            // Mappings pour la ressource Evenements
            // ----------------------------------------------------
            CreateMap<Evenement, EvenementDto>();  // AutoMapper mappera automatiquement TypeEvenement à TypeEvenementDto
            CreateMap<Evenement, EvenementUpdateDto>();  // 2. Entité vers DTO de mise à jour (pour l'initialisation du PATCH)
            CreateMap<EvenementCreateDto, Evenement>();  // 3. DTO de création vers Entité (POST)
            CreateMap<EvenementUpdateDto, Evenement>();// 4. DTO de mise à jour vers Entité (PUT/PATCH)

            // / ----------------------------------------------------
            // Mappings pour la ressource Actualite
            // ----------------------------------------------------
            CreateMap<Actualite, ActualiteDto>();  // AutoMapper mappera automatiquement Sensei à SenseiDto et Discipline à DisciplineDto
            CreateMap<Actualite, ActualiteUpdateDto>();  // 2. Entité
            CreateMap<ActualiteCreateDto, Actualite>();  // 3. DTO de création vers Entité (POST
            CreateMap<ActualiteUpdateDto, Actualite>()
                .ForMember(dest => dest.ActualiteId, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<User, UserDto>();


            // / ----------------------------------------------------
            // Mappings pour la ressource CategorieTransaction
            // ----------------------------------------------------
            CreateMap<CategorieTransaction, CategorieTransactionDto>();
            CreateMap<CategorieTransactionCreateDto, CategorieTransaction>();
            CreateMap<CategorieTransactionUpdateDto, CategorieTransaction>();
            CreateMap<CategorieTransaction, CategorieTransactionUpdateDto>();

            // / ----------------------------------------------------
            // Mappings pour la ressource Compte
            // ----------------------------------------------------
            CreateMap<Compte, CompteDto>();  // AutoMapper mappera automatiquement les Transactions à TransactionDto
            CreateMap<CompteCreateDto, Compte>();  // 2. DTO de création
            CreateMap<CompteUpdateDto, Compte>();// 3. DTO de mise à jour vers Entité (PUT/PATCH)
            CreateMap<Compte, CompteUpdateDto>();// 4. Entité vers DTO

            // / ----------------------------------------------------
            // Mappings pour la ressource Transaction
            // ----------------------------------------------------
            CreateMap<Transaction, TransactionDto>();  // AutoMapper mappera automatiquement Compte à CompteDto, CategorieTransaction à CategorieTransactionDto et Discipline à DisciplineDto
            CreateMap<TransactionCreateDto, Transaction>();  // 2. DTO de création vers Ent
            CreateMap<TransactionUpdateDto, Transaction>();// 3. DTO de mise à jour vers Entité (PUT/PATCH)
            CreateMap<Transaction, TransactionUpdateDto>();// 4. Entité vers DTO de mise

            // / ----------------------------------------------------
            // Mappings pour la ressource Tarif
            // ----------------------------------------------------
            CreateMap<Tarif, TarifDto>();  // AutoMapper mappera automatiquement Discipline à DisciplineDto
            CreateMap<TarifCreateDto, Tarif>();  // 2. DTO de création
            CreateMap<TarifUpdateDto, Tarif>();// 3. DTO de mise à jour vers Entité (PUT/PATCH)
            CreateMap<Tarif, TarifUpdateDto>();// 4. Entité vers DTO
        }
    }
}
