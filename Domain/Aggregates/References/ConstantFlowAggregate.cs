using Domain.Const;
using System;

namespace Domain.Aggregates.References
{
    public class ConstantFlowAggregate : ICalcaulationPeriod
    {
        public ConstantFlowAggregate(long id, long ownerId, string name, double consumption, VoltageLevel voltageLevelId, DateTime beginDate, DateTime endDate, bool isTransit, bool deleted)
        {
            Id = id;
            Name = name;
            Сonsumption = consumption;
            VoltageLevelId = voltageLevelId;
            BeginDate = beginDate;
            EndDate = endDate;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public long OwnerId { get; private set; }
        public string Name { get; private set; }
        public double Сonsumption { get; set; }
        public VoltageLevel VoltageLevelId { get; private set; }
        public bool IsTransit { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool Deleted { get; private set; }

        public double СalculationPeriod(DateTime period, DateTime beginDate, DateTime endDate)
        {
            var velocity = Сonsumption / DateTime.DaysInMonth(beginDate.Year, beginDate.Month);
            return (endDate.Date - beginDate.Date).TotalDays * velocity;
        }
    }
}