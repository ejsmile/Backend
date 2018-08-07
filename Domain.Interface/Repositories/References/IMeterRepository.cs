using Domain.Aggregates.References;
using System.Collections.Generic;

namespace Domain.Interface.Repositories.References
{
    public interface IMeterRepository : IRepositoryReferenes<MeterAggregate>
    {
        IEnumerable<MeterAggregate> GetByChannel(long id);

        IEnumerable<MeterAggregate> GetByChannel(ChannelAggregate channel);
    }
}