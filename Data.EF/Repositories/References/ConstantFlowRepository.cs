using AutoMapper;
using Data.EF.Models;
using Data.EF.Models.References;
using Domain.Aggregates.References;
using Domain.Const;
using Domain.Interface.Repositories.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.EF.Repositories.References
{
    public class ConstantFlowRepository : IConstantFlowRepository
    {
        private readonly DatabaseContext context;

        public ConstantFlowRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void Create(ConstantFlowAggregate item)
        {
            Create(item, false);
        }

        private void Create(ConstantFlowAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.ConstantFlowDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Constant flow id = {item.Id} found");
                var newItem = Mapper.Map<ConstantFlowDTO>(item);
                context.ConstantFlowDTOs.Add(newItem);
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
                var existed = context.ConstantFlowDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"Constant flow id = {id} found");
                existed.Deleted = true;
            });
        }

        public ConstantFlowAggregate Get(long id)
        {
            var item = context.ConstantFlowDTOs
                .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { flow = t, link = l })
                .Where(l => !l.link.IsTransit)
                .FirstOrDefault(v => v.flow.Id == id);

            if (item == null)
                throw new ArgumentException($"Constant flow id = {id} not found");

            var result = new ConstantFlowAggregate(item.link.Owner.Id, item.flow.Id, item.flow.Name, item.flow.Сonsumption, (VoltageLevel)item.flow.VoltageLevelId,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.flow.Deleted && item.link.Deleded);
            return result;
        }

        public IEnumerable<ConstantFlowAggregate> GetAll()
        {
            var items = context.ConstantFlowDTOs
              .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { flow = t, link = l })
            .ToList();

            return items.Select(item => new ConstantFlowAggregate(item.link.Owner.Id, item.flow.Id, item.flow.Name, item.flow.Сonsumption, (VoltageLevel)item.flow.VoltageLevelId,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.flow.Deleted && item.link.Deleded));
        }

        public IEnumerable<ConstantFlowAggregate> GetByTariffGroup(long id)
        {
            var items = context.ConstantFlowDTOs
                 .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { flow = t, link = l })
                .Where(lt => lt.link.Owner.Id == id)
              .ToList();

            return items.Select(item => new ConstantFlowAggregate(item.link.Owner.Id, item.flow.Id, item.flow.Name, item.flow.Сonsumption, (VoltageLevel)item.flow.VoltageLevelId,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.flow.Deleted && item.link.Deleded));
        }

        public IEnumerable<ConstantFlowAggregate> GetByTariffGroup(TariffGroupAggregate tariffGroup)
        {
            return GetByTariffGroup(tariffGroup.Id);
        }

        public void Update(ConstantFlowAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.ConstantFlowDTOs.SingleOrDefault(x => x.Id == item.Id);
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