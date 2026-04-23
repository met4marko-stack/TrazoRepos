using TrazoService.Domain.Entities.Templates;

namespace TrazoService.Domain.Entities;

public class Step : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsInitialStep { get; set; } 
    public bool IsFinalStep { get; set; }

    // Foreign Keys
    public int FlowchartId { get; set; }
    public WorkFlowsProccesType Flowchart { get; set; }
}