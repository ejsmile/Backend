using Data.EF.Models.References;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.Documents
{
    [Table("Indication")]
    public class IndicationDocumentDTO : DocumentDTO
    {
        public DateTime ReceiptDate { get; set; }

        [Required]
        public virtual MeterDTO Meter { get; set; }

        [Required]
        public virtual MeterScaleDTO Scale { get; set; }

        public double Indication { get; set; }
    }
}