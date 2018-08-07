using AutoMapper;
using Data.EF.Models.References;
using Domain.Aggregates.References;
using Domain.Interface.Repositories.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.EF.Repositories.References
{
    public class MeterTypeRepository : IMeterTypeRepository
    {
        private readonly DatabaseContext context;

        public MeterTypeRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void Create(MeterTypeAggregate item)
        {
            Create(item, false);
        }

        private void Create(MeterTypeAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.MeterTypeDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Meter type id = {item.Id} found");
                var newItem = Mapper.Map<MeterTypeDTO>(item);
                context.MeterTypeDTOs.Add(newItem);
            });
        }

        public void Delete(long id)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.AgreementDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"Meter type id = {id} found");
                existed.Deleted = true;
            });
        }

        public MeterTypeAggregate Get(long id)
        {
            var item = context.MeterTypeDTOs.FirstOrDefault(v => v.Id == id);

            if (item == null)
                throw new ArgumentException($"Meter type id = {id} not found");

            var result = new MeterTypeAggregate(item.Id, item.Name, item.ManufacturerName, item.ModelName, item.CalibrationIntervals, item.Deleted);
            return result;
        }

        public IEnumerable<MeterTypeAggregate> GetAll()
        {
            var items = context.MeterTypeDTOs.OrderBy(a => a.ManufacturerName).ThenBy(a => a.ModelName).ToList();
            return items.Select(item => new MeterTypeAggregate(item.Id, item.Name, item.ManufacturerName, item.ModelName, item.CalibrationIntervals, item.Deleted));
        }

        public void Update(MeterTypeAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.MeterTypeDTOs.SingleOrDefault(x => x.Id == item.Id);
                if (existed != null)
                    Mapper.Map(item, existed);
                else
                {
                    Create(item, true);
                }
            });
        }
    }
}