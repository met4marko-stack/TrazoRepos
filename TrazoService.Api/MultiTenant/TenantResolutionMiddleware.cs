using System.Text.Json;
using TrazoService.Infrastructure.MultiTenant;

namespace TrazoService.Api.MultiTenant;

public class TenantResolutionMiddleware
{
    private const string TenantIdQueryName = "tenantId";
    private const string TenantIdHeaderName = "X-Tenant-Id";
    private const string TenantIdClaimName = "tenantId";

    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext,
        ITenantService TenantService,
        ITenantConnectionAccessor tenantConnectionAccessor)
    {
        if (ShouldSkipTenantResolution(httpContext.Request.Path))
        {
            await _next(httpContext);
            return;
        }

        tenantConnectionAccessor.Clear();

        // 1. intentar obtener tenantId de claims por el token (para usuarios autenticados)
        string? rawTenantId = httpContext.User.FindFirst(TenantIdClaimName)?.Value;

        // 2. Intentar obtenerlo de Query o Headers
        if (string.IsNullOrWhiteSpace(rawTenantId))
        {
            rawTenantId = httpContext.Request.Query[TenantIdQueryName].FirstOrDefault()
                ?? httpContext.Request.Headers[TenantIdHeaderName].FirstOrDefault();
        }

        // 3. Intentar obtenerlo del Body
        if (string.IsNullOrWhiteSpace(rawTenantId))
        {
            rawTenantId = await GetTenantIdFromBody(httpContext);
        }

        if (string.IsNullOrWhiteSpace(rawTenantId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                message = "Es obligatorio enviar un 'tenantId' para acceder a este recurso."
            });
            return;
        }

        if (!Guid.TryParse(rawTenantId, out var tenantId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                message = "El tenantId enviado no es valido."
            });
            return;
        }

        var tenantCatalog = await TenantService.GetByIdAsync(tenantId, httpContext.RequestAborted);
        if (tenantCatalog is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                message = "No se encontro un tenant para el Guid enviado."
            });
            return;
        }

        tenantConnectionAccessor.SetTenant(tenantCatalog.Id, tenantCatalog.Index, tenantCatalog.ConnectionString);

        await _next(httpContext);
    }

    private async Task<string?> GetTenantIdFromBody(HttpContext httpContext)
    {
        if (httpContext.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) != true)
        {
            return null;
        }

        // Habilitar el buffering 
        httpContext.Request.EnableBuffering();

        try
        {
            // Leemos el stream sin cerrarlo para que el controlador pueda usarlo después
            using var reader = new StreamReader(
                httpContext.Request.Body, 
                encoding: System.Text.Encoding.UTF8, 
                detectEncodingFromByteOrderMarks: false, 
                leaveOpen: true);
            
            var body = await reader.ReadToEndAsync();
            
            // Reposicionamos el stream al inicio
            httpContext.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body)) return null;

            using var jsonDoc = JsonDocument.Parse(body);
            if (jsonDoc.RootElement.TryGetProperty("tenantId", out var tenantIdProp))
            {
                return tenantIdProp.GetString();
            }
        }
        catch
        {
            // Asegurando que el stream se resetee si hubo un error parcial
            if (httpContext.Request.Body.CanSeek)
            {
                httpContext.Request.Body.Position = 0;
            }
        }

        return null;
    }

    private static bool ShouldSkipTenantResolution(PathString path)
    {
        return path.StartsWithSegments("/swagger")
            || path.StartsWithSegments("/openapi");
    }
}
