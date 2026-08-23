using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASPPorcelette.API.Services
{
    public class SaisonStatisticsBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SaisonStatisticsBackgroundService(
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
                    using var scope = _scopeFactory.CreateScope();

                    var service = scope.ServiceProvider
                        .GetRequiredService<SaisonStatisticsService>();

                    await service.FreezePreviousSeasonAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Erreur lors du gel des statistiques : {ex.Message}");
                }

                // Vérification une fois par jour
                await Task.Delay(
                    TimeSpan.FromDays(1),
                    stoppingToken);
            }
        }
    }
}