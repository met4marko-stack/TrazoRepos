using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TrazoService.Domain.Entities;
using TrazoService.Infrastructure.Persistence;
using TrazoService.Infrastructure.Security;

namespace TrazoService.Infrastructure.MultiTenant;

public class TenantService : ITenantService
{
    private const string CacheKeyPrefix = "tenant:";

    private readonly TenantDbContext _TenantDbContext;
    private readonly IMemoryCache _cache;
    private readonly IEncryptionService _encryptionService;

    public TenantService(
        TenantDbContext TenantDbContext,
        IMemoryCache cache,
        IEncryptionService encryptionService)
    {
        _TenantDbContext = TenantDbContext;
        _cache = cache;
        _encryptionService = encryptionService;
    }

    public async Task<TenantCatalog?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CacheKeyPrefix}{tenantId}";

        if (_cache.TryGetValue(cacheKey, out TenantCatalogCacheEntry? cachedTenant) && cachedTenant is not null)
        {
            try
            {
                return new TenantCatalog
                {
                    Id = cachedTenant.Id,
                    Index = cachedTenant.Index,
                    ConnectionString = _encryptionService.Decrypt(cachedTenant.EncryptedConnectionString)
                };
            }
            catch
            {
                _cache.Remove(cacheKey);
            }
        }

        var tenant = await _TenantDbContext.TenantCatalogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == tenantId, cancellationToken);

        if (tenant is null)
        {
            return null;
        }

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

        _cache.Set(cacheKey, new TenantCatalogCacheEntry
        {
            Id = tenant.Id,
            Index = tenant.Index,
            EncryptedConnectionString = _encryptionService.Encrypt(tenant.ConnectionString)
        }, cacheEntryOptions);

        return tenant;
    }

    private sealed class TenantCatalogCacheEntry
    {
        public Guid Id { get; init; }
        public string Index { get; init; } = string.Empty;
        public string EncryptedConnectionString { get; init; } = string.Empty;
    }
}
