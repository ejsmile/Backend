using System;
using System.Collections.Generic;

namespace Domain.Aggregates.References
{
    public class UnitAggregate : ICalcaulationPeriod
    {
        private readonly List<TariffGroupAggregate> childrens = new List<TariffGroupAggregate>();

        public UnitAggregate(long ownerId, long id, string name, string adress, DateTime begDate, DateTime endDate, bool deleted)
        {
            OwnerId = ownerId;
            Id = id;
            Name = name;
            Adress = Adress;
            BeginDate = begDate;
            EndDate = endDate;
            Deleted = deleted;
        }

        public long OwnerId { get; private set; }
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Adress { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool Deleted { get; private set; }

        public IEnumerable<TariffGroupAggregate> Children
        {
            get { return childrens; }
        }

        public void AddChildren(TariffGroupAggregate tariffGroup)
        {
            if (tariffGroup.EndDate <= BeginDate || tariffGroup.BeginDate >= EndDate || tariffGroup.Deleted)
                return;
            childrens.Add(tariffGroup);
        }

        public double СalculationPeriod(DateTime period, DateTime beginDate, DateTime endDate)
        {
            if (endDate.Date <= BeginDate.Date || beginDate.Date >= EndDate.Date)
            {
                return 0.0;
            }
            var result = 0.0;
            foreach (var children in childrens)
            {
                result += (children.IsTransit ? -1 : 1)
                    * children.СalculationPeriod(period, beginDate > BeginDate ? beginDate : beginDate, endDate < EndDate ? endDate : endDate)
                    * children.Price;
            }
            return result;
        }
    }
}