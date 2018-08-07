using Domain.Aggregates.Document;
using Domain.Aggregates.References;
using Domain.Interface.Repositories.Documents;
using System.Collections.Generic;
using System.Linq;

namespace Data.EF.Repositories.Documents
{
    public class IndicationDocumentRepository : IIndicationDocumentRepository
    {
        private readonly DatabaseContext context;

        public IndicationDocumentRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public IEnumerable<IndicationDocumentAggregate> GetAllByMeterScale(MeterScaleAggregate meterScale)
        {
            var items = context.IndicationDocumentDTOs.Where(d => d.Scale.Id == meterScale.Id && !d.Deleted).ToList();
            return items.Select(item => new IndicationDocumentAggregate(item.Id, item.ReceiptDate, item.Indication, item.Deleted));
        }
    }
}