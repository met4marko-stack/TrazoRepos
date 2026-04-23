using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrazoService.Infrastructure.MultiTenant;
using TrazoService.Infrastructure.Persistence;
using TrazoService.Infrastructure.Security;
using TrazoService.Application.Interfaces.Repositories;
using TrazoService.Infrastructure.Persistence.Repository.Base;
using TrazoService.Infrastructure.Persistence.Repository;

namespace TrazoService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var TenantConnectionString = configuration.GetConnectionString("TenantConnection")
            ?? throw new InvalidOperationException("Connection string 'TenantConnection' no fue configurada.");

        // Registro de Seguridad y Cache
        var masterKey = configuration["MASTER_KEY_ENCRYPTION"] ?? "TrazoService_Default_Secure_Key_2026!";
        services.AddSingleton<IEncryptionService>(new EncryptionService(masterKey));
        services.AddMemoryCache();

        services.AddScoped<ITenantConnectionAccessor, TenantConnectionAccessor>();
        services.AddScoped<ITenantService, TenantService>();

        services.AddDbContext<TenantDbContext>(options =>
            options.UseSqlServer(TenantConnectionString));

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var tenantConnectionAccessor = serviceProvider.GetRequiredService<ITenantConnectionAccessor>();
            
            if (!tenantConnectionAccessor.HasTenant)
            {
                // En tiempo de diseño (migrations) o si no hay tenant, usamos una cadena vacía o lanzamos error
                // Para que las migraciones funcionen, EF necesita una cadena, pero en ejecución fallará si no hay tenant.
                options.UseSqlServer("Server=empty;Database=empty;Integrated Security=False;TrustServerCertificate=True;");
                return;
            }

            options.UseSqlServer(tenantConnectionAccessor.ConnectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        // Registro de Repositorios
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ICompanyRepository, CompanyRepository>();

        return services;
    }
}
