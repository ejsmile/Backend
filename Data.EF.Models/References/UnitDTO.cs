using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("Unit")]
    public class UnitDTO : ObjectDTO
    {
        [MaxLength(255)]
        public string Adress { get; set; }
    }
}