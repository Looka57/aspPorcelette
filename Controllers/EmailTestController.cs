using ASPPorcelette.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPPorcelette.API.Controllers
{
    [ApiController]
    [Route("api/email-test")]
    [Authorize(Roles = "Admin")]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendTestEmail()
        {
            var destinataire = "napoa322@gmail.com"; // Remplacez par l'adresse email de test souhaitée

            await _emailService.SendEmailAsync(
                destinataire,
                "Test email - AS Porcelette",
                """
                <h2>Test réussi !</h2>
                <p>Ceci est un email de test envoyé automatiquement par l'API de l'AS Porcelette.</p>
                <p>Si tu reçois ce message, la configuration Brevo fonctionne correctement.</p>
                """
            );

            return Ok(new
            {
                message = "Email envoyé avec succès."
            });
        }
    }
}