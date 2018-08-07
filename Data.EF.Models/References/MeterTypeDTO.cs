using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("MeterType")]
    public class MeterTypeDTO : ObjectDTO
    {
        [MaxLength(127)]
        public string ManufacturerName { get; set; }

        [MaxLength(127)]
        public string ModelName { get; set; }

        public byte CalibrationIntervals { get; set; }
    }
}