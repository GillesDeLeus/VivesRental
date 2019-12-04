using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VivesRental.Repository.Core;

namespace VivesRental.RestApi.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<VivesRentalDbContext>();
        }
    }
}
