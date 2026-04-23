using TrazoService.Domain.Entities.Templates;

namespace TrazoService.Domain.Entities;

public class WorkFlowsProccesType : BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Version { get; set; } 
}