using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VivesRental.Services;
using VivesRental.Services.Contracts;

namespace VivesRental.RestApi.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IRentalItemService, RentalItemService>();
            services.AddScoped<IRentalOrderLineService, RentalOrderLineService>();
            services.AddScoped<IRentalOrderService, RentalOrderService>();
        }
    }
}
