using Domain.Aggregates.References;
using System.Collections.Generic;

namespace Domain.Interface.Repositories.References
{
    public interface IConstantFlowRepository : IRepositoryReferenes<ConstantFlowAggregate>
    {
        IEnumerable<ConstantFlowAggregate> GetByTariffGroup(long id);

        IEnumerable<ConstantFlowAggregate> GetByTariffGroup(TariffGroupAggregate tariffGroup);
    }
}