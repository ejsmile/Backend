using System;

namespace Domain.Aggregates.Document
{
    public class IndicationDocumentAggregate
    {
        public IndicationDocumentAggregate(long id, DateTime receiptDate, double indication, bool deleted)
        {
            Id = id;
            ReceiptDate = receiptDate;
            Indication = indication;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public DateTime ReceiptDate { get; private set; }
        public double Indication { get; private set; }
        public bool Deleted { get; private set; }
    }
}