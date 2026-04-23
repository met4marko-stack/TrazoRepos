using Microsoft.EntityFrameworkCore;
using TrazoService.Domain.Entities;

namespace TrazoService.Infrastructure.Persistence;

public class TenantDbContext : DbContext
{
    public const string TableName = "tenantDb";

    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
    {
    }

    public DbSet<TenantCatalog> TenantCatalogs { get; set; } = null!;

    public Task EnsureCatalogTableAsync(string mainTenantConnectionString, CancellationToken cancellationToken = default)
    {
        string sql = $"""
            IF OBJECT_ID(N'[tenantDb]', N'U') IS NULL
            BEGIN
                CREATE TABLE [tenantDb] (
                    [Id] uniqueidentifier NOT NULL,
                    [ConnectionString] nvarchar(2048) NOT NULL,
                    [Index] nvarchar(128) NOT NULL,
                    CONSTRAINT [PK_tenantDb] PRIMARY KEY ([Id])
                );
            END;

            IF NOT EXISTS (
                SELECT 1
                FROM sys.indexes
                WHERE name = N'IX_tenantDb_Index'
                AND object_id = OBJECT_ID(N'[tenantDb]')
            )
            BEGIN
                CREATE UNIQUE INDEX [IX_tenantDb_Index] ON [tenantDb]([Index]);
            END;

            IF NOT EXISTS (
                SELECT 1
                FROM [tenantDb]
                WHERE [Id] = 'f6b3e7a0-2e1a-4c5d-9b8a-1f2e3d4c5b6a'
            )
            BEGIN
                INSERT INTO [tenantDb] ([Id], [Index], [ConnectionString])
                VALUES (
                    'f6b3e7a0-2e1a-4c5d-9b8a-1f2e3d4c5b6a',
                    N'main',
                    N'{mainTenantConnectionString}'
                );
            END;
            """;

        return Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TenantCatalog>(entity =>
        {
            entity.ToTable(TableName);
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ConnectionString)
                .IsRequired()
                .HasMaxLength(2048);

            entity.Property(x => x.Index)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasIndex(x => x.Index)
                .IsUnique();
        });

        // Seed Data para el catálogo de tenants
        modelBuilder.Entity<TenantCatalog>().HasData(
            new TenantCatalog
            {
                Id = Guid.Parse("f6b3e7a0-2e1a-4c5d-9b8a-1f2e3d4c5b6a"),
                Index = "main",
                ConnectionString = "Server=localhost;Database=TrazoCliente1Db;Integrated Security=True;TrustServerCertificate=True;"
            }
        );

        modelBuilder.Entity<TenantCatalog>().HasData(
            new TenantCatalog {
                Id = Guid.Parse("f6b3e7a0-2e1a-4c5d-9b8a-1f2e3d4c5b6b"), 
                Index = "cliente2",
                ConnectionString = "Server=localhost;Database=TrazoCliente2Db;Integrated Security=True;TrustServerCertificate=True;"
            }
        );

        modelBuilder.Entity<TenantCatalog>().HasData(
            new TenantCatalog {
                Id = Guid.Parse("f6b3e7a0-2e1a-4c5d-9b8a-1f2e3d4c5b6c"), 
                Index = "cliente3",
                ConnectionString = "Server=localhost;Database=TrazoCliente3Db;Integrated Security=True;TrustServerCertificate=True;"
            }
        );
    }
}
