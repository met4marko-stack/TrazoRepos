namespace TrazoService.Domain.Entities;

public class ConnectionStep
{
    public int Id { get; set; }
    public bool IsDefault { get; set; }
    
    public string Description { get; set; }

    // Foreign Keys
    public int StepId { get; set; }
    public Step Step { get; set; }

    public int TargetStepId { get; set; }
    public Step TargetStep { get; set; }
}