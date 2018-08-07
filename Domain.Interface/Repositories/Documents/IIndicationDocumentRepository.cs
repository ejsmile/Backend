using Domain.Aggregates.Document;
using Domain.Aggregates.References;
using System.Collections.Generic;

namespace Domain.Interface.Repositories.Documents
{
    public interface IIndicationDocumentRepository
    {
        IEnumerable<IndicationDocumentAggregate> GetAllByMeterScale(MeterScaleAggregate meterScale);
    }
}