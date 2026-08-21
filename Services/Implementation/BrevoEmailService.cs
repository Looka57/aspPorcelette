using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ASPPorcelette.API.Services.Interfaces;

namespace ASPPorcelette.API.Services.Implementation
{
    public class BrevoEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public BrevoEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string destinataire,
            string sujet,
            string contenu)
        {
            var smtpHost = _configuration["Brevo:SmtpHost"];
            var smtpPort = int.Parse(_configuration["Brevo:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Brevo:SmtpUser"];
            var smtpKey = _configuration["Brevo:SmtpKey"];

            var fromEmail = _configuration["Brevo:FromEmail"];
            var fromName = _configuration["Brevo:FromName"] ?? "AS Porcelette";

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(fromName, fromEmail));
            email.To.Add(MailboxAddress.Parse(destinataire));
            email.Subject = sujet;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = contenu
            };

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                smtpHost,
                smtpPort,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                smtpUser,
                smtpKey);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}