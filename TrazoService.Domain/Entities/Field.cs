namespace TrazoService.Domain.Entities;

public class Field
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; } 
    
    // Note: '1=Text 2=Number 3=Date 4=DateTime 5=Select 6=MultiSelect 7=Boolean 8=TextArea 9=File 10=Signature'
    public string Type { get; set; }
    
    public string Placeholder { get; set; }
    public int DefaultValueNumber { get; set; }
    public string DefaultValueText { get; set; }
    public DateTime DefaultValueDate { get; set; }
    public bool DefaultValueBool { get; set; }
    public bool IsGlobal { get; set; }

    // Foreign Keys
    public int ParentFielId { get; set; }
    public Field ParentField { get; set; }
}