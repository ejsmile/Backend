using Domain.Aggregates.References;

namespace Domain.Interface.Repositories.References
{
    public interface ITariffRepository : IRepositoryReferenes<TariffAggregate>
    {
        void Create(long id, string name, string category, double price, bool deleted);

        void Update(long id, string name, string category, double price, bool deleted);
    }
}