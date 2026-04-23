namespace TrazoService.Infrastructure.MultiTenant;

public interface ITenantConnectionAccessor
{
    Guid? TenantId { get; }
    string? TenantIndex { get; }
    string? ConnectionString { get; }
    bool HasTenant { get; }
    void SetTenant(Guid tenantId, string tenantIndex, string connectionString);
    void Clear();
}
