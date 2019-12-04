using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VivesRental.RestApi.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
