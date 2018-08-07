using Domain.Aggregates.References;
using System.Collections.Generic;

namespace Domain.Interface.Repositories.References
{
    public interface IUnitRepository : IRepositoryReferenes<UnitAggregate>
    {
        IEnumerable<UnitAggregate> GetByAgreement(long id);

        IEnumerable<UnitAggregate> GetByAgreement(AgreementAggregate agreement);
    }
}