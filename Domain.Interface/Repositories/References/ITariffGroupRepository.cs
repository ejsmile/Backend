using Domain.Aggregates.References;
using System.Collections.Generic;

namespace Domain.Interface.Repositories.References
{
    public interface ITariffGroupRepository : IRepositoryReferenes<TariffGroupAggregate>
    {
        IEnumerable<TariffGroupAggregate> GetByUnit(long id);

        IEnumerable<TariffGroupAggregate> GetByUnit(UnitAggregate unit);
    }
}