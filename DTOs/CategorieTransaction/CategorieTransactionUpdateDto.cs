
// DTO de Mise à Jour (PUT/PATCH)
using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.Models;

 public class CategorieTransactionUpdateDto
    {
        [MaxLength(150)]
        public string? Nom { get; set; }
        
        // L'enum est nullable pour le PATCH si on ne veut pas le changer
        public TypeFlux? TypeFlux { get; set; }
    }