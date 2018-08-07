using AutoMapper;
using Data.EF.Models.References;
using Domain.Aggregates.References;
using Domain.Interface.Repositories.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.EF.Repositories.References
{
    public class TariffRepository : ITariffRepository
    {
        private readonly DatabaseContext context;

        public TariffRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void Create(TariffAggregate item)
        {
            Create(item, false);
        }

        private void Create(TariffAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.TariffDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Tariff id = {item.Id} found");
                var newItem = Mapper.Map<TariffDTO>(item);
                context.TariffDTOs.Add(newItem);
            });
        }

        public void Delete(long id)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.TariffDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"Tariff id = {id} found");
                existed.Deleted = true;
            });
        }

        public TariffAggregate Get(long id)
        {
            var item = context.TariffDTOs.FirstOrDefault(v => v.Id == id);

            if (item == null)
                throw new ArgumentException($"Tariff id = {id} not found");

            var result = new TariffAggregate(item.Id, item.Name, item.Category, item.Price, item.Deleted);
            return result;
        }

        public IEnumerable<TariffAggregate> GetAll()
        {
            var items = context.TariffDTOs.OrderBy(a => a.Name).ToList();
            return items.Select(item => new TariffAggregate(item.Id, item.Name, item.Category, item.Price, item.Deleted));
        }

        public void Update(TariffAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.TariffDTOs.SingleOrDefault(x => x.Id == item.Id);
                if (existed != null)
                    Mapper.Map(item, existed);
                else
                {
                    Create(item, true);
                }
            });
        }

        public void Create(long id, string name, string category, double price, bool deleted)
        {
            var item = new TariffAggregate(id, name, category, price, deleted);
            Create(item);
        }

        public void Update(long id, string name, string category, double price, bool deleted)
        {
            var item = new TariffAggregate(id, name, category, price, deleted);
            Update(item);
        }
    }
}