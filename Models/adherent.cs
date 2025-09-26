class Adherent
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public DateTime DateNaissance { get; set; }
    public string Adresse { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public DateTime DateAdhesion { get; set; }
    public DateTime DateRenouvellement { get; set; }
    public bool EstActif { get; set; }
    public bool Statut { get; set; } // true = actif, false = inactif

    
}