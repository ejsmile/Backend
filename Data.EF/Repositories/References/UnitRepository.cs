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
    public class UnitRepository : IUnitRepository
    {
        private readonly DatabaseContext context;
        private readonly ITariffGroupRepository childrenRepository;

        public UnitRepository(DatabaseContext context, ITariffGroupRepository childrenRepository)
        {
            this.context = context;
            this.childrenRepository = childrenRepository;
        }

        public void Delete(long id)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.UnitDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"Unit id = {id} found");
                existed.Deleted = true;
            });
        }

        public UnitAggregate Get(long id)
        {
            var item = context.UnitDTOs
                .Join(context.LinkObjectsDTOs, u => u.Id, l => l.Childen.Id, (u, l) => new { unit = u, link = l })
                .FirstOrDefault(v => v.unit.Id == id);

            if (item == null)
                throw new ArgumentException($"Unit id = {id} not found");

            var result = new UnitAggregate(item.link.Owner.Id, item.unit.Id, item.unit.Name, item.unit.Adress,
                item.link.BeginDate, item.link.EndDate, item.unit.Deleted && item.link.Deleded);
            return result;
        }

        public IEnumerable<UnitAggregate> GetAll()
        {
            var items = context.UnitDTOs
               .Join(context.LinkObjectsDTOs, u => u.Id, l => l.Childen.Id, (u, l) => new { unit = u, link = l })
               .ToList();

            return items.Select(item => new UnitAggregate(item.link.Owner.Id, item.unit.Id, item.unit.Name, item.unit.Adress,
                item.link.BeginDate, item.link.EndDate, item.unit.Deleted && item.link.Deleded));
        }

        public IEnumerable<UnitAggregate> GetByAgreement(long id)
        {
            var items = context.UnitDTOs
                .Join(context.LinkObjectsDTOs, u => u.Id, l => l.Childen.Id, (u, l) => new { unit = u, link = l })
                .Where(v => v.link.Owner.Id == id).ToList();

            var result = new List<UnitAggregate>();

            foreach (var item in items.Select(item => new UnitAggregate(item.link.Owner.Id, item.unit.Id, item.unit.Name, item.unit.Adress,
                item.link.BeginDate, item.link.EndDate, item.unit.Deleted && item.link.Deleded)))
            {
                foreach (var child in childrenRepository.GetByUnit(item))
                {
                    item.AddChildren(child);
                }
                result.Add(item);
            }

            return result;
        }

        public IEnumerable<UnitAggregate> GetByAgreement(AgreementAggregate agreement)
        {
            return GetByAgreement(agreement.Id);
        }

        public void Update(UnitAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.UnitDTOs.SingleOrDefault(x => x.Id == item.Id);
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

        public void Create(UnitAggregate item)
        {
            Create(item, false);
        }

        private void Create(UnitAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.UnitDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Unit id = {item.Id} found");
                var newItem = Mapper.Map<UnitDTO>(item);
                context.UnitDTOs.Add(newItem);
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
    }
}