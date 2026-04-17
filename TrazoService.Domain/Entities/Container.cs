namespace TrazoService.Domain.Entities;

public class Container
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int OrganizationId { get; set; }
    public int ParentFormId { get; set; }

    // Foreign Keys
    public int StepId { get; set; }
    public Step Step { get; set; }
}