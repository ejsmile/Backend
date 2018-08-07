using Domain.Const;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Aggregates.References
{
    public class ChannelAggregate : ICalcaulationPeriod
    {
        private readonly List<MeterAggregate> childrens = new List<MeterAggregate>();

        public ChannelAggregate(long id, long ownerId, string name, VoltageLevel voltageLevelId, DateTime beginDate, DateTime endDate, bool isTransit, bool deleted)
        {
            Id = id;
            Name = name;
            VoltageLevelId = voltageLevelId;
            BeginDate = beginDate;
            EndDate = endDate;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public long OwnerId { get; private set; }
        public string Name { get; private set; }
        public VoltageLevel VoltageLevelId { get; private set; }
        public bool IsTransit { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool Deleted { get; private set; }

        public void AddChildren(MeterAggregate meter)
        {
            if (meter.EndDate <= BeginDate || meter.BeginDate >= EndDate || meter.Deleted)
                return;
            childrens.Add(meter);
            if (childrens.Where(c => !c.IsTransit).Count() > 1)
            {
                throw new AggregateException("Number meter more one");
            }
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
                    * children.СalculationPeriod(period, beginDate > BeginDate ? beginDate : beginDate, endDate < EndDate ? endDate : endDate);
            }
            return result;
        }
    }
}