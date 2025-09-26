class Transaction
{
    public int Id { get; set; }
    public decimal Montant { get; set; }
    public DateTime DateTransaction { get; set; }
    public string Description { get; set; }

    
    public int CategorieTransactionId { get; set; }
    public CategorieTransaction CategorieTransaction { get; set; }
}