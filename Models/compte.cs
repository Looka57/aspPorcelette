class Compte
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public decimal Solde { get; set; }
    public string TypeCompte { get; set; } // Par exemple : "Épargne", "Chèque", etc.
    
    public DateTime DateCreation { get; set; }
   
    
}