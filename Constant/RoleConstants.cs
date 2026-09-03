namespace ASPPorcelette.API.Constants
{
    /// <summary>
    /// Classe statique définissant les noms de rôle utilisés dans l'application.
    /// Cela garantit que les noms de rôles sont toujours cohérents et sans faute de frappe.
    /// </summary>
    public static class RoleConstants
    {
        // Gestion complète de l'application
        public const string Admin = "Admin";

        // Utilisateurs pouvant accéder au dashboard
        public const string Sensei = "Sensei";
        public const string Comite = "Comité";
        public const string Secretaire = "Secrétaire";
        public const string Tresoriere = "Trésorière";

        // Licenciés : accès uniquement à leur espace personnel
        public const string Adherent = "Adherent";

        // Ancien rôle conservé 
        public const string Student = "Student";

        // Ancien rôle comptable conservé temporairement
        public const string Comptable = "Comptable";
    }
}