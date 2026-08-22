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

        private const string KimonoImageUrl =
            "https://img.icons8.com/plasticine/100/kimono.png";

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
            return await _context.Users
                .Where(u =>
                    u.Statut == 1 &&
                    u.CertificatMedicalFourni &&
                    u.DateExpirationCertificatMedical.HasValue &&
                    !string.IsNullOrEmpty(u.Email))
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

                var dateRappelJ30 = dateExpiration.AddDays(-30);
                var dateRappelJ7 = dateExpiration.AddDays(-7);
                var dateRappelExpire = dateExpiration.AddDays(1);

                // ========================================================
                // J-30
                // ========================================================

                if (today == dateRappelJ30)
                {
                    var contenu = BuildEmail(
                        user,
                        "Votre certificat médical arrive bientôt à expiration",
                        "30 jours",
                        dateExpiration,
                        "anticipation");

                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "AS Porcelette - Rappel certificat médical (J-30)",
                        contenu);

                    Console.WriteLine(
                        $"✅ Rappel J-30 envoyé à {user.Email}");
                }

                // ========================================================
                // J-7
                // ========================================================

                else if (today == dateRappelJ7)
                {
                    var contenu = BuildEmail(
                        user,
                        "Votre certificat médical expire bientôt",
                        "7 jours",
                        dateExpiration,
                        "attention");

                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "AS Porcelette - Votre certificat médical expire dans 7 jours",
                        contenu);

                    Console.WriteLine(
                        $"✅ Rappel J-7 envoyé à {user.Email}");
                }

                // ========================================================
                // J+1
                // ========================================================

                else if (today == dateRappelExpire)
                {
                    var contenu = BuildEmail(
                        user,
                        "Votre certificat médical est arrivé à expiration",
                        null,
                        dateExpiration,
                        "urgent");

                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "AS Porcelette - URGENT : certificat médical expiré",
                        contenu);

                    Console.WriteLine(
                        $"🚨 Mail certificat expiré envoyé à {user.Email}");
                }
            }
        }

        // ================================================================
        // CONSTRUCTION DU MAIL
        // ================================================================

        private string BuildEmail(
            User user,
            string titre,
            string? delai,
            DateTime dateExpiration,
            string type)
        {
            var couleur = type switch
            {
                "anticipation" => "#2563EB",
                "attention" => "#D97706",
                "urgent" => "#DC2626",
                _ => "#1F2937"
            };

            var fond = type switch
            {
                "anticipation" => "#EFF6FF",
                "attention" => "#FFF7ED",
                "urgent" => "#FEF2F2",
                _ => "#F3F4F6"
            };

            var messagePrincipal = type switch
            {
                "anticipation" => $"""
                    Votre certificat médical arrivera à expiration
                    dans <strong>{delai}</strong>.
                    """,

                "attention" => $"""
                    Votre certificat médical arrive à expiration
                    dans <strong>{delai}</strong>.
                    """,

                "urgent" => """
                    Votre certificat médical est désormais
                    <strong>expiré</strong>.
                    """,

                _ => ""
            };

            var action = type switch
            {
                "anticipation" => """
                    Nous vous invitons à anticiper son renouvellement
                    afin d'éviter toute interruption de votre pratique
                    sportive au sein de l'AS Porcelette.
                    """,

                "attention" => """
                    Merci de nous transmettre votre nouveau certificat
                    médical dans les meilleurs délais afin d'éviter toute
                    interruption de votre pratique sportive.
                    """,

                "urgent" => """
                    Merci de transmettre un nouveau certificat médical
                    valide au club avant toute reprise de votre pratique
                    sportive.
                    """,

                _ => ""
            };

            return $"""
            <!DOCTYPE html>
            <html lang="fr">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>AS Porcelette - Certificat médical</title>
            </head>

            <body style="
                margin:0;
                padding:0;
                background-color:#f3f4f6;
                font-family:Arial, Helvetica, sans-serif;
                color:#1f2937;
            ">

                <table
                    width="100%"
                    cellpadding="0"
                    cellspacing="0"
                    border="0"
                    style="background-color:#f3f4f6; padding:30px 15px;"
                >
                    <tr>
                        <td align="center">

                            <table
                                width="600"
                                cellpadding="0"
                                cellspacing="0"
                                border="0"
                                style="
                                    max-width:600px;
                                    width:100%;
                                    background-color:#ffffff;
                                    border-radius:10px;
                                    overflow:hidden;
                                "
                            >

                                <!-- ================================================= -->
                                <!-- EN-TÊTE -->
                                <!-- ================================================= -->

                                <tr>
                                    <td
                                        style="
                                            background-color:#111827;
                                            padding:25px 30px;
                                            text-align:center;
                                        "
                                    >
                                        <img
                                            src="{KimonoImageUrl}"
                                            width="70"
                                            height="70"
                                            alt="Arts martiaux"
                                            style="
                                                display:block;
                                                margin:0 auto 12px auto;
                                            "
                                        >

                                        <div style="
                                            color:#ffffff;
                                            font-size:22px;
                                            font-weight:bold;
                                            letter-spacing:1px;
                                        ">
                                            AS PORCELETTE
                                        </div>

                                        <div style="
                                            color:#d1d5db;
                                            font-size:13px;
                                            margin-top:5px;
                                        ">
                                            Arts martiaux
                                        </div>
                                    </td>
                                </tr>

                                <!-- ================================================= -->
                                <!-- CONTENU -->
                                <!-- ================================================= -->

                                <tr>
                                    <td style="padding:35px 35px 25px 35px;">

                                        <div style="
                                            font-size:22px;
                                            font-weight:bold;
                                            color:#111827;
                                            margin-bottom:20px;
                                        ">
                                            {titre}
                                        </div>

                                        <p style="
                                            font-size:15px;
                                            line-height:1.7;
                                            margin:0 0 18px 0;
                                        ">
                                            Bonjour {user.Prenom} {user.Nom},
                                        </p>

                                        <p style="
                                            font-size:15px;
                                            line-height:1.7;
                                            margin:0 0 22px 0;
                                        ">
                                            {messagePrincipal}
                                        </p>

                                        <!-- ================================================= -->
                                        <!-- DATE -->
                                        <!-- ================================================= -->

                                        <table
                                            width="100%"
                                            cellpadding="0"
                                            cellspacing="0"
                                            border="0"
                                            style="
                                                background-color:{fond};
                                                border-left:4px solid {couleur};
                                                margin:20px 0;
                                            "
                                        >
                                            <tr>
                                                <td style="padding:18px 20px;">

                                                    <div style="
                                                        font-size:12px;
                                                        color:#6b7280;
                                                        text-transform:uppercase;
                                                        letter-spacing:.5px;
                                                        margin-bottom:6px;
                                                    ">
                                                        Date d'expiration
                                                    </div>

                                                    <div style="
                                                        font-size:24px;
                                                        font-weight:bold;
                                                        color:{couleur};
                                                    ">
                                                        {dateExpiration:dd/MM/yyyy}
                                                    </div>

                                                </td>
                                            </tr>
                                        </table>

                                        <p style="
                                            font-size:15px;
                                            line-height:1.7;
                                            margin:25px 0 0 0;
                                        ">
                                            {action}
                                        </p>

                                        <!-- ================================================= -->
                                        <!-- SIGNATURE -->
                                        <!-- ================================================= -->

                                        <p style="
                                            font-size:15px;
                                            line-height:1.7;
                                            margin:28px 0 0 0;
                                        ">
                                            Sportivement,<br>
                                            <strong>AS Porcelette</strong>
                                        </p>

                                    </td>
                                </tr>

                                <!-- ================================================= -->
                                <!-- FOOTER -->
                                <!-- ================================================= -->

                                <tr>
                                    <td
                                        style="
                                            background-color:#f9fafb;
                                            border-top:1px solid #e5e7eb;
                                            padding:20px 30px;
                                            text-align:center;
                                        "
                                    >
                                        <div style="
                                            font-size:12px;
                                            color:#6b7280;
                                            line-height:1.6;
                                        ">
                                            AS Porcelette<br>
                                            Arts martiaux
                                        </div>

                                        <div style="
                                            font-size:11px;
                                            color:#9ca3af;
                                            margin-top:8px;
                                        ">
                                            Ceci est un message automatique.
                                            Merci de ne pas répondre directement
                                            à cet e-mail.
                                        </div>
                                    </td>
                                </tr>

                            </table>

                        </td>
                    </tr>
                </table>

            </body>
            </html>
            """;
        }
    }
}

