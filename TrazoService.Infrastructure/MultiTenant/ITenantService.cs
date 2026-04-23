using TrazoService.Domain.Entities;

namespace TrazoService.Infrastructure.MultiTenant;

public interface ITenantService
{
    Task<TenantCatalog?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
