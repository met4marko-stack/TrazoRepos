using AuthModule.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using TrazoService.Infrastructure.Persistence; 
using Microsoft.Extensions.Logging;

namespace TrazoService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Recuperamos la plantilla de conexión del appsettings.json
        /*var connectionTemplate = configuration.GetConnectionString("TenantTemplateConnection");

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            // Inyectamos el Accessor que viene del paquete Noxun.Auth.Shared
            var tenantAccessor = serviceProvider.GetRequiredService<ITenantConnectionAccessor>();
            var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            if (tenantAccessor.HasTenant && !string.IsNullOrEmpty(tenantAccessor.DatabaseName))
            {
                var dynamicConnectionString = connectionTemplate?.Replace("{DbName}", tenantAccessor.DatabaseName);
                
                options.UseSqlServer(dynamicConnectionString, sqlOptions => 
                {
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
            }
            else
            {
                logger.LogWarning("Intento de conexión a BD sin Tenant válido. Se bloqueó el acceso apuntando a 'NO_TENANT'.");
                throw new UnauthorizedAccessException("Se requiere un Tenant válido para acceder a la base de datos de Trazo.");
            }
        });*/

        // NOTA: Si más adelante creas Repositorios (ej. ICompanyRepository), 
        // puedes registrarlos aquí mismo usando:
        // services.AddScoped<ICompanyRepository, CompanyRepository>();

        return services;
    }
}