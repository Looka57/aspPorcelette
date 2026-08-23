using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASPPorcelette.API.Services
{
    public class MedicalCertificateReminderHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MedicalCertificateReminderHostedService(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // ============================================================
                    // ATTENDRE JUSQU'À 9H00
                    // ============================================================

                    var now = DateTime.Now;

                    var prochaineExecution = now.Date.AddHours(10).AddMinutes(10);

                    // Si 9h est déjà passé aujourd'hui,
                    // on programme l'exécution pour demain à 9h.
                    if (now >= prochaineExecution)
                    {
                        prochaineExecution = prochaineExecution.AddDays(1);
                    }

                    var delai = prochaineExecution - now;

                    Console.WriteLine(
                        $"⏰ Prochaine vérification des certificats : " +
                        $"{prochaineExecution:dd/MM/yyyy HH:mm}");

                    await Task.Delay(delai, stoppingToken);

                    Console.WriteLine(
    $"🚀 Délai terminé : {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

                    // ============================================================
                    // ENVOI DES RAPPELS
                    // ============================================================

                    using var scope = _scopeFactory.CreateScope();

                    var reminderService =
                        scope.ServiceProvider
                            .GetRequiredService<MedicalCertificateReminderService>();

                    await reminderService.SendRemindersAsync();

                    Console.WriteLine(
                        $"✅ Vérification des certificats terminée : " +
                        $"{DateTime.Now:dd/MM/yyyy HH:mm}");
                }
                catch (OperationCanceledException)
                {
                    // Arrêt normal de l'application
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"❌ Erreur lors de l'envoi automatique des rappels : {ex.Message}");

                    // En cas d'erreur, on attend 10 minutes
                    // avant de tenter à nouveau.
                    try
                    {
                        await Task.Delay(
                            TimeSpan.FromMinutes(10),
                            stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }
    }
}