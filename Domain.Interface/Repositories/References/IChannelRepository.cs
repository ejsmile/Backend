using Domain.Aggregates.References;
using System.Collections.Generic;

namespace Domain.Interface.Repositories.References
{
    public interface IChannelRepository : IRepositoryReferenes<ChannelAggregate>
    {
        IEnumerable<ChannelAggregate> GetByTariffGroup(long id);

        IEnumerable<ChannelAggregate> GetByTariffGroup(TariffGroupAggregate tariffGroup);
    }
}