using Domain.Aggregates.References;
using System.Collections.Generic;

namespace Domain.Interface.Repositories.References
{
    public interface IMeterScaleRepostitory : IRepositoryReferenes<MeterScaleAggregate>
    {
        IEnumerable<MeterScaleAggregate> GetByMeter(long id);

        IEnumerable<MeterScaleAggregate> GetByMeter(MeterAggregate meter);
    }
}