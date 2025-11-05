using System.ComponentModel.DataAnnotations;

public class TransactionTransferDto
{
    public int SourceCompteId { get; set; }
    public int DestinationCompteId { get; set; }

    public decimal Montant { get; set; }
    public string Description { get; set; }
    public int? CategorieId { get; set; } // facultatif : pour le suivi

     public int DisciplineId { get; set; } // obligatoire maintenant
}
