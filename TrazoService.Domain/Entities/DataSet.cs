namespace TrazoService.Domain.Entities;

public class DataSet
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } 
    public string ValueText { get; set; }
    public decimal ValueNumber { get; set; }
    public DateTime ValueDate { get; set; }
    public bool ValueBoolean { get; set; }
    public DateTime RegisterDate { get; set; }
    public int Col { get; set; }

    // Foreign Keys
    public string ParentFieldName { get; set; }
    
    public int FieldId { get; set; }
    public Field Field { get; set; }

    public int ProcedureId { get; set; }
    public Procedure Procedure { get; set; }

    public int DataSetRowId { get; set; }
    public DataSetRow DataSetRow { get; set; }
}