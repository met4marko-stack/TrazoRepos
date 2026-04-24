using AuthModule.Extensions;
using AuthModule.Data;
using AuthModule.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TrazoService.Infrastructure;
using TrazoService.Application;
using TrazoService.Infrastructure.Persistence;
using TrazoService.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthModuleSetup(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddTrazoApplicationServices();
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();
builder.Services.AddDbContext<TenantDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("TenantConnection")));
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


app.UseAuthorization();

app.MapControllers();

app.Run();

