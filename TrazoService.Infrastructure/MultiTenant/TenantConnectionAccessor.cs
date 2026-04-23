namespace TrazoService.Infrastructure.MultiTenant;

public class TenantConnectionAccessor : ITenantConnectionAccessor
{
    public Guid? TenantId { get; private set; }
    public string? TenantIndex { get; private set; }
    public string? ConnectionString { get; private set; }
    public bool HasTenant => TenantId.HasValue && !string.IsNullOrWhiteSpace(ConnectionString);

    public void SetTenant(Guid tenantId, string tenantIndex, string connectionString)
    {
        TenantId = tenantId;
        TenantIndex = tenantIndex;
        ConnectionString = connectionString;
    }

    public void Clear()
    {
        TenantId = null;
        TenantIndex = null;
        ConnectionString = null;
    }
}
