namespace TrazoService.Domain.Entities;

public class Procedure
{
    public int Id { get; set; }
    public string Code { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public string Carpeta { get; set; }
    
    public string WorkFlowsProccesTypeId { get; set; }
    public string Status { get; set; }
    
    public int OrganizationId { get; set; }
    public int NodeStepId { get; set; }
    public DateTime RegisterDate { get; set; }

    // Foreign Keys
    public int CategoryCod { get; set; }
    public WorkFlowsProccesType Category { get; set; } 

    public int CompanyId { get; set; }
    public Company Company { get; set; } 

    public int StepId { get; set; }
    public Step Step { get; set; }
}