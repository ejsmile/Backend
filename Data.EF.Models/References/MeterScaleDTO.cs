using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("MeterScale")]
    public class MeterScaleDTO : ObjectDTO
    {
        public byte ZoneOfDayId { get; set; }
        public byte Dimension { get; set; }
    }
}