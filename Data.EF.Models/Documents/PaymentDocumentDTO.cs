using Data.EF.Models.References;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.Documents
{
    [Table("PaymentDocuments")]
    public class PaymentDocumentDTO : DocumentDTO
    {
        public DateTime CalculationPeriod { get; set; }
        public DateTime WiredPeriod { get; set; }
        public bool Wired { get; set; }
        public AgreementDTO Agreement { get; set; }
        public double Amount { get; set; }
    }
}