using System.ComponentModel.DataAnnotations;

namespace TrazoService.Domain.Entities;

public class TenantCatalog
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(2048)]
    public string ConnectionString { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string Index { get; set; } = string.Empty;
}
