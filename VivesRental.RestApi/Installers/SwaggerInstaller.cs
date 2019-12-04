using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VivesRental.RestApi.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "MUD API", Version = "v1" });
                setup.ExampleFilters();
                AddSecurityDefinition(setup);
                AddSecurityRequirement(setup);
                AddXmlDocumentation(setup);
            });
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }

        private void AddSecurityDefinition(SwaggerGenOptions setup)
        {
            var securityDefinitionScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            };

            setup.AddSecurityDefinition("Bearer", securityDefinitionScheme);
        }

        private void AddSecurityRequirement(SwaggerGenOptions setup)
        {
            var securityRequirementScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            var security = new OpenApiSecurityRequirement
            {
                {
                    securityRequirementScheme, new string[] { }
                }
            };
            setup.AddSecurityRequirement(security);
        }

        private void AddXmlDocumentation(SwaggerGenOptions setup)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            setup.IncludeXmlComments(xmlPath);
        }

    }
}
