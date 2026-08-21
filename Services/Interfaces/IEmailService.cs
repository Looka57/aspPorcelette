namespace ASPPorcelette.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string destinataire,
            string sujet,
            string contenu);
    }
}