using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(CheckEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }

        public static string CheckEnvironmentVariable(string envVariable)
        {
            var env = Environment.GetEnvironmentVariable(envVariable);

            if (env == "Production")
                return "ProductionConnection";

            if (env == "Development")
                return "DevelopmentConnection";

            return default;
        }
    }
}
