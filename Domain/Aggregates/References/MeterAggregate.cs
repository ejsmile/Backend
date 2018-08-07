using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Aggregates.References
{
    public class MeterAggregate : ICalcaulationPeriod
    {
        private readonly List<MeterScaleAggregate> childrens = new List<MeterScaleAggregate>();

        public MeterAggregate(long id, long ownerId, string name, string publicNumber, string serialNumber, byte countScale, DateTime beginDate, DateTime endDate, bool isTransit, bool deleted)
        {
            Id = id;
            Name = name;
            SerialNumber = publicNumber;
            SerialNumber = serialNumber;
            CountScale = countScale;
            BeginDate = beginDate;
            EndDate = endDate;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public long OwnerId { get; private set; }
        public string Name { get; private set; }
        public string PublicNumber { get; private set; }
        public string SerialNumber { get; private set; }
        public byte CountScale { get; private set; }
        public bool IsTransit { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool Deleted { get; private set; }

        public void AddChildren(MeterScaleAggregate meterScale)
        {
            if (meterScale.EndDate <= BeginDate || meterScale.BeginDate >= EndDate || meterScale.Deleted)
                return;
            childrens.Add(meterScale);
            if (childrens.Where(c => !c.IsTransit).Count() > CountScale)
            {
                throw new AggregateException("Number Scale more CountScale");
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