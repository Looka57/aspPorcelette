namespace ASPPorcelette.API.Models.Enums
{
    // Définit la nature de l'impact financier de la transaction.
    public enum TypeFlux
    {
        // L'argent sort
        Depense, 
        
        // L'argent rentre
        Revenu,  
        
        // Ex: Transfert inter-compte, ajustement sans impact sur le total général.
        Neutre 
    }
}
