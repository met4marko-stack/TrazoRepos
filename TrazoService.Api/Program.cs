using AuthModule.Extensions;
using AuthModule.Data;
using AuthModule.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TrazoService.Api.MultiTenant;
using TrazoService.Api.Swagger;
using TrazoService.Infrastructure;
using TrazoService.Application;
using TrazoService.Infrastructure.MultiTenant;
using TrazoService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthModuleSetup(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddTrazoApplicationServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AuthDbContext>((serviceProvider, options) =>
{
    var tenantConnectionAccessor = serviceProvider.GetRequiredService<ITenantConnectionAccessor>();
    
    if (tenantConnectionAccessor.HasTenant)
    {
        options.UseSqlServer(tenantConnectionAccessor.ConnectionString);
    }
    else
    {
        // Solo permitimos una conexión válida si estamos en el proceso de inicialización (MigrateAsync)
        // En peticiones HTTP normales, tenantConnectionAccessor.HasTenant será falso si no hay tenant,
        // y esto causará un error de conexión a 'empty', protegiendo los datos.
        //options.UseSqlServer("Server=empty;Database=empty;Integrated Security=False;TrustServerCertificate=True;");
        options.UseSqlServer("server=localhost;database=TrazoCliente3Db;TrustServerCertificate=True;Trusted_Connection=true;");
    }
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{   
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TrazoService API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });

    var baseDirectory = AppContext.BaseDirectory;
    var files = Directory.EnumerateFiles(baseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
    foreach (var file in files)
    {
        c.IncludeXmlComments(file);
    }

    c.OperationFilter<TenantLoginOperationFilter>();
    c.OperationFilter<TenantRegisterOperationFilter>();
});



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();

    var tenantCatalogContext = services.GetRequiredService<TenantDbContext>();
    
    // 0. Aseguramos que la base de datos del catálogo exista
    await tenantCatalogContext.Database.EnsureCreatedAsync();
    
    // Obtenemos la cadena de conexión deseada desde el appsettings.json
    var mainTenantConn = configuration.GetConnectionString("MainTenantConnection") 
        ?? "Server=localhost;Database=TrazoDb;Integrated Security=True;TrustServerCertificate=True;"; 

    await tenantCatalogContext.EnsureCatalogTableAsync(mainTenantConn);
    
    // Migrar las bases de datos de todos los tenants registrados
    var tenants = await tenantCatalogContext.TenantCatalogs.ToListAsync();
    foreach (var tenant in tenants)
    {
        using var tenantScope = app.Services.CreateScope();
        var tenantServices = tenantScope.ServiceProvider;
        
        var tenantAccessor = tenantServices.GetRequiredService<ITenantConnectionAccessor>();
        tenantAccessor.SetTenant(tenant.Id, tenant.Index, tenant.ConnectionString);

        var tenantAuthContext = tenantServices.GetRequiredService<AuthDbContext>();
        await tenantAuthContext.Database.MigrateAsync();

        var tenantApplicationContext = tenantServices.GetRequiredService<ApplicationDbContext>();
        await tenantApplicationContext.Database.MigrateAsync();

        // Sembrar datos para cada tenant
        var seedService = tenantServices.GetRequiredService<SeedDataService>();
        await seedService.SeedAsync();
    }
}

app.Run();

