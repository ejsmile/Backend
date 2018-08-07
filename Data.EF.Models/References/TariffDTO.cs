using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("Tariff")]
    public class TariffDTO : ObjectDTO
    {
        [MaxLength(100)]
        public string Category { get; set; }

        public double Price { get; set; }
    }
}