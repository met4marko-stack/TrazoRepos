namespace TrazoService.Domain.Entities;

public class ConditionGroup
{
    public int Id { get; set; }
    public string Description { get; set; }
    // Foreign Keys
    public int ConnectionStepsId { get; set; }
    public ConnectionStep ConnectionStep { get; set; }
} 