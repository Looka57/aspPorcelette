namespace ASPPorcelette.API.Model
{
    // Ce modèle correspond à la section "JwtSettings" de appsettings.json
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
