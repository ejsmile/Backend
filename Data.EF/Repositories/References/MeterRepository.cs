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
    public class MeterRepository : IMeterRepository
    {
        private readonly IMeterScaleRepostitory childrenRepository;
        private readonly DatabaseContext context;

        public MeterRepository(DatabaseContext context, IMeterScaleRepostitory childrenRepository)
        {
            this.context = context;
            this.childrenRepository = childrenRepository;
        }

        public void Create(MeterAggregate item)
        {
            Create(item, false);
        }

        private void Create(MeterAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.MeterDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Channel id = {item.Id} found");
                var newItem = Mapper.Map<MeterDTO>(item);
                context.MeterDTOs.Add(newItem);
                var link = new LinkObjectsDTO()
                {
                    Owner = context.ObjectDTOs.Single(o => o.Id == item.OwnerId),
                    Childen = newItem,
                    BeginDate = item.BeginDate,
                    EndDate = item.EndDate,
                    IsTransit = false,
                };
                context.LinkObjectsDTOs.Add(link);
            });
        }

        public void Delete(long id)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.MeterDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"MeterScale id = {id} found");
                existed.Deleted = true;
            });
        }

        public MeterAggregate Get(long id)
        {
            var item = context.MeterDTOs
                 .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { meter = t, link = l })
                 .Where(l => !l.link.IsTransit)
                 .FirstOrDefault(v => v.meter.Id == id);

            if (item == null)
                throw new ArgumentException($"Channel id = {id} not found");

            var result = new MeterAggregate(item.meter.Id, item.link.Owner.Id, item.meter.Name, item.meter.PublicNumber, item.meter.SerialNumber, item.meter.CountScale,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.meter.Deleted && item.link.Deleded);

            return result;
        }

        public IEnumerable<MeterAggregate> GetAll()
        {
            var items = context.MeterDTOs
            .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { meter = t, link = l })
            .ToList();

            return items.Select(item => new MeterAggregate(item.meter.Id, item.link.Owner.Id, item.meter.Name, item.meter.PublicNumber, item.meter.SerialNumber, item.meter.CountScale,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.meter.Deleted && item.link.Deleded));
        }

        public IEnumerable<MeterAggregate> GetByChannel(long id)
        {
            var items = context.MeterDTOs
            .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { meter = t, link = l })
            .ToList();

            var result = new List<MeterAggregate>();

            foreach (var item in items.Select(item => new MeterAggregate(item.meter.Id, item.link.Owner.Id, item.meter.Name, item.meter.PublicNumber, item.meter.SerialNumber, item.meter.CountScale,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.meter.Deleted && item.link.Deleded)))
            {
                foreach (var children in childrenRepository.GetByMeter(item))
                {
                    item.AddChildren(children);
                }
                result.Add(item);
            }
            return result;
        }

        public IEnumerable<MeterAggregate> GetByChannel(ChannelAggregate channel)
        {
            return GetByChannel(channel.Id);
        }

        public void Update(MeterAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.MeterDTOs.SingleOrDefault(x => x.Id == item.Id);
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