using AutoMapper;
using Data.EF.Models;
using Data.EF.Models.References;
using Domain.Aggregates.References;
using Domain.Interface.Repositories.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.EF.Repositories.References
{
    public class TariffGroupRepository : ITariffGroupRepository
    {
        private readonly DatabaseContext context;
        private readonly IConstantFlowRepository childrenConstantRepository;
        private readonly IChannelRepository childrenChannelRepository;

        public TariffGroupRepository(DatabaseContext context, IConstantFlowRepository childrenConstantRepository, IChannelRepository childrenChannelRepository)
        {
            this.context = context;
            this.childrenConstantRepository = childrenConstantRepository;
            this.childrenChannelRepository = childrenChannelRepository;
        }

        public void Create(TariffGroupAggregate item)
        {
            Create(item, false);
        }

        private void Create(TariffGroupAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.TariffGroupDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Tariff Group id = {item.Id} found");
                var newItem = Mapper.Map<TariffGroupDTO>(item);
                context.TariffGroupDTOs.Add(newItem);
                var link = new LinkObjectsDTO()
                {
                    Owner = context.ObjectDTOs.Single(o => o.Id == item.OwnerId),
                    Childen = newItem,
                    BeginDate = item.BeginDate,
                    EndDate = item.EndDate,
                    IsTransit = false,
                };
                context.LinkObjectsDTOs.Add(link);
                return newItem;
            });
        }

        public void Delete(long id)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.TariffGroupDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"TariffGroup id = {id} found");
                existed.Deleted = true;
            });
        }

        public TariffGroupAggregate Get(long id)
        {
            var item = context.TariffGroupDTOs
                .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { trgroup = t, link = l })
                .Where(l => !l.link.IsTransit)
                .FirstOrDefault(v => v.trgroup.Id == id);

            if (item == null)
                throw new ArgumentException($"Tariff Group id = {id} not found");

            var result = new TariffGroupAggregate(item.link.Owner.Id, item.trgroup.Id, item.trgroup.Name, item.trgroup.Tariff.Name, item.trgroup.Tariff.Price,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.trgroup.Deleted && item.link.Deleded);

            return result;
        }

        public IEnumerable<TariffGroupAggregate> GetAll()
        {
            var items = context.TariffGroupDTOs
              .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { trgroup = t, link = l })
            .ToList();

            return items.Select(item => new TariffGroupAggregate(item.link.Owner.Id, item.trgroup.Id, item.trgroup.Name, item.trgroup.Tariff.Name, item.trgroup.Tariff.Price,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.trgroup.Deleted && item.link.Deleded));
        }

        public IEnumerable<TariffGroupAggregate> GetByUnit(long id)
        {
            var items = context.TariffGroupDTOs
                .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { trgroup = t, link = l })
                .Where(lt => lt.link.Owner.Id == id)
              .ToList();

            var result = new List<TariffGroupAggregate>();

            foreach (var item in items.Select(item => new TariffGroupAggregate(item.link.Owner.Id, item.trgroup.Id, item.trgroup.Name, item.trgroup.Tariff.Name, item.trgroup.Tariff.Price,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.trgroup.Deleted && item.link.Deleded)))
            {
                foreach (var child in childrenConstantRepository.GetByTariffGroup(item))
                {
                    item.AddChildrenConst(child);
                }
                foreach (var child in childrenChannelRepository.GetByTariffGroup(item))
                {
                    item.AddChildrenChannel(child);
                }
                result.Add(item);
            }
            return result;
        }

        public IEnumerable<TariffGroupAggregate> GetByUnit(UnitAggregate unit)
        {
            return GetByUnit(unit.Id);
        }

        public void Update(TariffGroupAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.TariffGroupDTOs.SingleOrDefault(x => x.Id == item.Id);
                if (existed != null)
                {
                    Mapper.Map(item, existed);
                    var link = context.LinkObjectsDTOs.Single(l => l.Owner.Id == item.OwnerId && l.Childen.Id == item.Id);
                    Mapper.Map(item, link);
                }
                else
                {
                    Create(item, true);
                }
            });
        }
    }
}