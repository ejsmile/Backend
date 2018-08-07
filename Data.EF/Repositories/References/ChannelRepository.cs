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
    public class ChannelRepository : IChannelRepository
    {
        private readonly IMeterRepository childrenRepository;
        private readonly DatabaseContext context;

        public ChannelRepository(DatabaseContext context, IMeterRepository childrenRepository)
        {
            this.context = context;
            this.childrenRepository = childrenRepository;
        }

        public void Create(ChannelAggregate item)
        {
            Create(item, false);
        }

        public void Delete(long id)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.ChannelDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"Channel id = {id} found");
                existed.Deleted = true;
            });
        }

        public ChannelAggregate Get(long id)
        {
            var item = context.ChannelDTOs
                 .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { channel = t, link = l })
                 .Where(l => !l.link.IsTransit)
                 .FirstOrDefault(v => v.channel.Id == id);

            if (item == null)
                throw new ArgumentException($"Channel id = {id} not found");

            var result = new ChannelAggregate(item.channel.Id, item.link.Owner.Id, item.channel.Name, (VoltageLevel)item.channel.VoltageLevelId,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.channel.Deleted && item.link.Deleded);

            return result;
        }

        public IEnumerable<ChannelAggregate> GetAll()
        {
            var items = context.ChannelDTOs
            .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { channel = t, link = l })
            .ToList();

            return items.Select(item => new ChannelAggregate(item.channel.Id, item.link.Owner.Id, item.channel.Name, (VoltageLevel)item.channel.VoltageLevelId,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.channel.Deleted && item.link.Deleded));
        }

        public IEnumerable<ChannelAggregate> GetByTariffGroup(long id)
        {
            var items = context.ChannelDTOs
                 .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { channel = t, link = l })
                 .Where(l => l.link.Owner.Id == id).ToList();

            var result = new List<ChannelAggregate>();

            foreach (var item in items.Select(item => new ChannelAggregate(item.channel.Id, item.link.Owner.Id, item.channel.Name, (VoltageLevel)item.channel.VoltageLevelId,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.channel.Deleted && item.link.Deleded)))
            {
                foreach (var children in childrenRepository.GetByChannel(item))
                {
                    item.AddChildren(children);
                }
                result.Add(item);
            }
            return result;
        }

        public IEnumerable<ChannelAggregate> GetByTariffGroup(TariffGroupAggregate tariffGroup)
        {
            return GetByTariffGroup(tariffGroup.Id);
        }

        public void Update(ChannelAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.ChannelDTOs.SingleOrDefault(x => x.Id == item.Id);
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

        private void Create(ChannelAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.ChannelDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Channel id = {item.Id} found");
                var newItem = Mapper.Map<ChannelDTO>(item);
                context.ChannelDTOs.Add(newItem);
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
    }
}