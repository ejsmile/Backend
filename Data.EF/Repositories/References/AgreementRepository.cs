using AutoMapper;
using Data.EF.Models.References;
using Domain.Aggregates.References;
using Domain.Interface.Repositories.References;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Data.EF.Repositories.References
{
    public class AgreementRepository : IAgreementRepository
    {
        private readonly DatabaseContext context;
        private readonly IUnitRepository childrenRepository;

        public AgreementRepository(DatabaseContext context, IUnitRepository childrenRepository)
        {
            this.context = context;
            this.childrenRepository = childrenRepository;
        }

        public void Create(AgreementAggregate item)
        {
            Create(item, false);
        }

        private void Create(AgreementAggregate item, bool notCheck)
        {
            context.ExecuteInTransaction(() =>
            {
                if (!notCheck && (item.Id != -1 || context.AgreementDTOs.SingleOrDefault(x => x.Id == item.Id) != null))
                    throw new ArgumentException($"Agreement id = {item.Id} found");
                var newItem = Mapper.Map<AgreementDTO>(item);
                context.AgreementDTOs.Add(newItem);
            });
        }

        public void Create(long id, string name, string number, DateTime begDate, DateTime endDate, bool deleted)
        {
            var item = new AgreementAggregate(id, name, number, begDate, endDate, deleted);
            Create(item);
        }

        public void Delete(long id)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.AgreementDTOs.SingleOrDefault(x => x.Id == id);
                if (existed != null)
                    throw new ArgumentException($"Agreement id = {id} found");
                existed.Deleted = true;
            });
        }

        public AgreementAggregate Get(long id)
        {
            var item = context.AgreementDTOs.FirstOrDefault(v => v.Id == id);

            if (item == null)
                throw new ArgumentException($"Agreement id = {id} not found");

            var result = new AgreementAggregate(item.Id, item.Name, item.Number, item.BegDate, item.EndDate, item.Deleted);
            foreach (var child in childrenRepository.GetByAgreement(result))
            {
                result.AddChildren(child);
            }
            return result;
        }

        public IEnumerable<AgreementAggregate> GetAll()
        {
            var items = context.AgreementDTOs.OrderBy(a => a.Number).ThenBy(a => a.BegDate).ToList();
            return items.Select(i => new AgreementAggregate(i.Id, i.Name, i.Number, i.BegDate, i.EndDate, i.Deleted));
        }

        public void Update(long id, string name, string number, DateTime begDate, DateTime endDate, bool deleted)
        {
            var item = new AgreementAggregate(id, name, number, begDate, endDate, deleted);
            Update(item);
        }

        public void Update(AgreementAggregate item)
        {
            context.ExecuteInTransaction(() =>
            {
                var existed = context.AgreementDTOs.SingleOrDefault(x => x.Id == item.Id);
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