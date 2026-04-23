using TrazoService.Domain.Entities.Templates;

namespace TrazoService.Domain.Entities;

public class DataSetRow : BaseEntity
{
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