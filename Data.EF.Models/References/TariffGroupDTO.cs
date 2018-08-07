using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("TariffGroup")]
    public class TariffGroupDTO : ObjectDTO
    {
        public virtual TariffDTO Tariff { get; set; }
    }
}