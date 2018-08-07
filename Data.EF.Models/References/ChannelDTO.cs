using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("Channel")]
    public class ChannelDTO : ObjectDTO
    {
        public byte VoltageLevelId { get; set; }
    }
}