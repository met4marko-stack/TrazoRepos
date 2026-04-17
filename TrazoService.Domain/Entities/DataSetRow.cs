namespace TrazoService.Domain.Entities;

public class DataSetRow
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Name { get; set; }
    
    // orden de la fila
    public int Row { get; set; }

    // Foreign Keys
    public int FieldId { get; set; }
    public Field Field { get; set; }

    public int ProcedureId { get; set; }
    public Procedure Procedure { get; set; }
}