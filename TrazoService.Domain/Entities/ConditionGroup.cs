using TrazoService.Domain.Entities.Templates;

namespace TrazoService.Domain.Entities;

public class ConditionGroup : BaseEntity
{
    public string Description { get; set; }
    // Foreign Keys
    public int ConnectionStepsId { get; set; }
    public ConnectionStep ConnectionStep { get; set; }
} 