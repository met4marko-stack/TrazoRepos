namespace TrazoService.Domain.Entities;

public class StepField
{
    public int Id { get; set; }
    public int Order { get; set; }
    public bool IsRequired { get; set; }
    public string ValidationRuleJson { get; set; }

    // Foreign Keys
    public int FieldId { get; set; }
    public Field Field { get; set; }

    public int StepId { get; set; }
    public Step Step { get; set; }
}