    using Microsoft.OpenApi;
    using Swashbuckle.AspNetCore.SwaggerGen;

    namespace TrazoService.Api.Swagger;

    public class TenantLoginOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var relativePath = context.ApiDescription.RelativePath;
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return;
            }

            var isLoginEndpoint = relativePath.Contains("auth/login", StringComparison.OrdinalIgnoreCase);
            if (!isLoginEndpoint)
            {
                return;
            }

            operation.Parameters ??= [];

            if (operation.Parameters.Any(x => string.Equals(x.Name, "tenantId", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "tenantId",
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Format = "uuid"
                }
            });
        }
    }
