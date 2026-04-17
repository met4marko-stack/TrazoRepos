namespace TrazoService.Domain.Entities;

public class Condition
{
    public int Id { get; set; }
    
    public string Operator { get; set; }
    
    public string CompareValueText { get; set; }
    public string CompareValueNumber { get; set; }
    public string CompareValueDate { get; set; }
    public string CompareValueBoolean { get; set; }
    public string TypeCompare { get; set; }    
    public string ContextVariable { get; set; } // valor para tomar company en lugar de un field específico
    public int TypeCondition { get; set; } // Para escoger si usar el campo FieldId o ContextVariable

    // Foreign Keys
    public int ConditionGroupId { get; set; }
    public ConditionGroup ConditionGroup { get; set; }

    public int FieldId { get; set; }
    public Field Field { get; set; }
}