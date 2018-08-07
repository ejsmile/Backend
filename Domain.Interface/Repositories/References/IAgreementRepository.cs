using Domain.Aggregates.References;
using System;

namespace Domain.Interface.Repositories.References
{
    public interface IAgreementRepository : IRepositoryReferenes<AgreementAggregate>
    {
        void Create(long id, string name, string number, DateTime begDate, DateTime endDate, bool deleted);

        void Update(long id, string name, string number, DateTime begDate, DateTime endDate, bool deleted);
    }
}