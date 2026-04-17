namespace TrazoService.Domain.Entities;

public class ProcedureHistory
{
    public int Id { get; set; }
    public string Status { get; set; }
    public DateTime TimeIn { get; set; }    
    public DateTime TimeOut { get; set; }    
    public string AuditEvents { get; set; }

    // Foreign Keys
    public int ProcedureId { get; set; }
    public Procedure Procedure { get; set; }

    public int NodeStepId { get; set; }
    public Step NodeStep { get; set; }
}