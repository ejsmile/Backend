using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("ConstantFlow")]
    public class ConstantFlowDTO : ObjectDTO
    {
        public double Сonsumption { get; set; }
        public byte VoltageLevelId { get; set; }
    }
}