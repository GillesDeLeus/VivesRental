using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VivesRental.Repository;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;

namespace VivesRental.RestApi.Installers
{
    public class RepositoriesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IRentalItemRepository, RentalItemRepository>();
            services.AddScoped<IRentalOrderLineRepository, RentalOrderLineRepository>();
            services.AddScoped<IRentalOrderRepository, RentalOrderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
