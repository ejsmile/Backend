using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("Meter")]
    public class MeterDTO : ObjectDTO
    {
        [MaxLength(16)]
        public string PublicNumber { get; set; }

        [MaxLength(48)]
        public string SerialNumber { get; set; }

        public byte CountScale { get; set; }
    }
}