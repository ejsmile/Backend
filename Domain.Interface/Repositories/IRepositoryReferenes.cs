using System.Collections.Generic;

namespace Domain.Interface.Repositories
{
    public interface IRepositoryReferenes<T> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(long id);

        void Create(T item);

        void Update(T item);

        void Delete(long id);
    }
}