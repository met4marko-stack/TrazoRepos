using TrazoService.Domain.Entities.Templates;

namespace TrazoService.Domain.Entities;

public class StepRole : BaseEntity
{
    // Foreign Keys
    public int StepId { get; set; }
    public Step Step { get; set; }
}