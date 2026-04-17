namespace TrazoService.Domain.Entities;

public class ContainerField
{
    public int Id { get; set; }
    public int OrdenPresentation { get; set; }
    public int IsRequire { get; set; }

    // Foreign Keys
    public int FieldId { get; set; }
    public Field Field { get; set; }

    public int ContainerId { get; set; }
    public Container Container { get; set; }
}