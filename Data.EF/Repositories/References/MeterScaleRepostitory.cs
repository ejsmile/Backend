using AutoMapper;
using Data.EF.Models;
using Data.EF.Models.References;
using Domain.Aggregates.References;
using Domain.Const;
using Domain.Interface.Repositories.Documents;
using Domain.Interface.Repositories.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.EF.Repositories.References
{
    public class MeterScaleRepostitory : IMeterScaleRepostitory
    {
        private readonly DatabaseContext context;
        private readonly IIndicationDocumentRepository documentRepository;

        public MeterScaleRepostitory(DatabaseContext context, IIndicationDocumentRepository documentRepository)
        {
            this.context = context;
            this.documentRepository = documentRepository;
        }

        public void Create(MeterScaleAggregate item)
        {
            Create(item, false);
        }

        private void Create(MeterScaleAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.MeterScaleDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"MeterScale id = {item.Id} found");
                var newItem = Mapper.Map<MeterScaleDTO>(item);
                context.MeterScaleDTOs.Add(newItem);
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
                var existed = context.MeterScaleDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"MeterScale id = {id} found");
                existed.Deleted = true;
            });
        }

        public MeterScaleAggregate Get(long id)
        {
            var item = context.MeterScaleDTOs
                .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { scale = t, link = l })
                .Where(l => !l.link.IsTransit)
                .FirstOrDefault(v => v.scale.Id == id);

            if (item == null)
                throw new ArgumentException($"MeterScale id = {id} not found");

            var result = new MeterScaleAggregate(item.scale.Id, item.link.Owner.Id, item.scale.Name, (ZoneOfDay)item.scale.ZoneOfDayId, item.scale.Dimension,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.scale.Deleted && item.link.Deleded);

            return result;
        }

        public IEnumerable<MeterScaleAggregate> GetAll()
        {
            var items = context.MeterScaleDTOs
             .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { scale = t, link = l })
           .ToList();

            return items.Select(item => new MeterScaleAggregate(item.scale.Id, item.link.Owner.Id, item.scale.Name, (ZoneOfDay)item.scale.ZoneOfDayId, item.scale.Dimension,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.scale.Deleted && item.link.Deleded));
        }

        public void Update(MeterScaleAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.MeterScaleDTOs.SingleOrDefault(x => x.Id == item.Id);
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

        public IEnumerable<MeterScaleAggregate> GetByMeter(long id)
        {
            var items = context.MeterScaleDTOs
             .Join(context.LinkObjectsDTOs, t => t.Id, l => l.Childen.Id, (t, l) => new { scale = t, link = l })
             .Where(l => l.link.Owner.Id == id)
             .ToList();

            var result = new List<MeterScaleAggregate>();

            foreach (var item in items.Select(item => new MeterScaleAggregate(item.scale.Id, item.link.Owner.Id, item.scale.Name, (ZoneOfDay)item.scale.ZoneOfDayId, item.scale.Dimension,
                item.link.BeginDate, item.link.EndDate, item.link.IsTransit, item.scale.Deleted && item.link.Deleded)))
            {
                foreach (var i in documentRepository.GetAllByMeterScale(item))
                {
                    item.AddIndication(i);
                }
                result.Add(item);
            }
            return result;
        }

        public IEnumerable<MeterScaleAggregate> GetByMeter(MeterAggregate meter)
        {
            return GetByMeter(meter.Id);
        }
    }
}