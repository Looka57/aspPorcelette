using Microsoft.EntityFrameworkCore;
using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Services.Interfaces;

namespace ASPPorcelette.API.Services
{
    public class MedicalCertificateReminderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public MedicalCertificateReminderService(
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ================================================================
        // UTILISATEURS CONCERNÉS
        // ================================================================

        public async Task<List<User>> GetUsersToRemindAsync()
        {
            var today = DateTime.Today;

            return await _context.Users
                .Where(u =>
                    u.Statut == 1 &&
                    u.CertificatMedicalFourni &&
                    u.DateExpirationCertificatMedical.HasValue &&
                    u.Email != null &&
                    u.Email != "")
                .ToListAsync();
        }

        // ================================================================
        // ENVOI DES RAPPELS
        // ================================================================

        public async Task SendRemindersAsync()
        {
            var today = DateTime.Today;
            var users = await GetUsersToRemindAsync();

            foreach (var user in users)
            {
                if (!user.DateExpirationCertificatMedical.HasValue)
                    continue;

                var dateExpiration =
                    user.DateExpirationCertificatMedical.Value.Date;

                var dateRappelJ30 =
                    dateExpiration.AddDays(-30);

                var dateRappelJ7 =
                    dateExpiration.AddDays(-7);

                var dateRappelExpire =
                    dateExpiration.AddDays(1);

                // ============================================================
                // 📧 RAPPEL J-30
                // ============================================================

                if (today == dateRappelJ30)
                {
                    var contenu = $"""
                <h2>Renouvellement de votre certificat médical</h2>

                <p>Bonjour {user.Prenom} {user.Nom},</p>

                <p>
                    Votre certificat médical arrivera prochainement
                    à expiration.
                </p>

                <p>
                    Date d'expiration :
                    <strong>{dateExpiration:dd/MM/yyyy}</strong>
                </p>

                <p>
                    Merci de fournir un nouveau certificat médical
                    afin de pouvoir continuer votre pratique au sein
                    de l'AS Porcelette.
                </p>

                <p>
                    Cordialement,<br>
                    AS Porcelette
                </p>
                """;

                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "Rappel - Certificat médical",
                        contenu);

                    Console.WriteLine(
                        $"✅ Rappel J-30 envoyé à {user.Email}");
                }

                // ============================================================
                // 📧 RAPPEL J-7
                // ============================================================

                else if (today == dateRappelJ7)
                {
                    var contenu = $"""
                <h2>Votre certificat médical expire bientôt</h2>

                <p>Bonjour {user.Prenom} {user.Nom},</p>

                <p>
                    Votre certificat médical arrivera à expiration
                    dans <strong>7 jours</strong>.
                </p>

                <p>
                    Date d'expiration :
                    <strong>{dateExpiration:dd/MM/yyyy}</strong>
                </p>

                <p>
                    Pensez à fournir votre nouveau certificat médical
                    afin de pouvoir continuer votre pratique au sein
                    de l'AS Porcelette.
                </p>

                <p>
                    Cordialement,<br>
                    AS Porcelette
                </p>
                """;

                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "Important - Certificat médical bientôt expiré",
                        contenu);

                    Console.WriteLine(
                        $"✅ Rappel J-7 envoyé à {user.Email}");
                }

                // ============================================================
                // 🚨 CERTIFICAT EXPIRÉ
                // ============================================================

                else if (today == dateRappelExpire)
                {
                    var contenu = $"""
                <h2>Votre certificat médical est expiré</h2>

                <p>Bonjour {user.Prenom} {user.Nom},</p>

                <p>
                    Votre certificat médical est désormais
                    <strong>officiellement expiré</strong>.
                </p>

                <p>
                    Date d'expiration :
                    <strong>{dateExpiration:dd/MM/yyyy}</strong>
                </p>

                <p>
                    Vous devez fournir un nouveau certificat médical
                    avant de pouvoir continuer votre pratique au sein
                    de l'AS Porcelette.
                </p>

                <p>
                    Merci de prendre les dispositions nécessaires
                    auprès du club.
                </p>

                <p>
                    Cordialement,<br>
                    AS Porcelette
                </p>
                """;

                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "URGENT - Certificat médical expiré",
                        contenu);

                    Console.WriteLine(
                        $"🚨 Mail certificat expiré envoyé à {user.Email}");
                }
            }
        }
    }
}