using TrazoService.Domain.Entities.Templates;

namespace TrazoService.Domain.Entities;

public class ContainerField : BaseEntity
{
    public int OrdenPresentation { get; set; }
    public int IsRequire { get; set; }

    // Foreign Keys
    public int FieldId { get; set; }
    public Field Field { get; set; }

    public int ContainerId { get; set; }
    public Container Container { get; set; }
}