using System.ComponentModel.DataAnnotations;

namespace TrazoService.Domain.Entities.Templates;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public int? DeletedById { get; set; }

    public BaseEntity()
    {
        CreatedAt = DateTime.Now;
    }

    public virtual void MarkAsDeleted(int? deletedByUserId = null)
    {
        DeletedAt = DateTime.Now;
        DeletedById = deletedByUserId;
        UpdateTimestamp(deletedByUserId);
    }

    public bool IsDeleted => DeletedAt == null;

    public virtual void UpdateTimestamp(int? updatedByUserId = null)
    {
        UpdatedAt = DateTime.Now;
        UpdatedById = updatedByUserId;
    }
}
