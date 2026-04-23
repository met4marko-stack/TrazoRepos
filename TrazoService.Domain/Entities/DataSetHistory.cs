using TrazoService.Domain.Entities.Templates;

namespace TrazoService.Domain.Entities;

public class DataSetHistory : BaseEntity
{
    public string ValueCurrent { get; set; }
    public string ValueBefore { get; set; } 
    
    public string Description { get; set; }

    // Foreign Keys
    public int DataSetId { get; set; }
    public DataSet DataSet { get; set; }
}