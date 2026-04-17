namespace TrazoService.Domain.Entities;

public class StepRole
{
    public int Id { get; set; }

    // Foreign Keys
    public int StepId { get; set; }
    public Step Step { get; set; }
}