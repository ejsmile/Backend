using System;
using System.Collections.Generic;

namespace Domain.Aggregates.References

{
    public class AgreementAggregate : ICalcaulationPeriod
    {
        private readonly List<UnitAggregate> children = new List<UnitAggregate>();

        public AgreementAggregate(long id, string name, string number, DateTime begDate, DateTime endDate, bool deleted)
        {
            Id = id;
            Name = name;
            Number = number;
            BeginDate = begDate;
            EndDate = endDate;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Number { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool Deleted { get; private set; }

        public IEnumerable<UnitAggregate> Children
        {
            get { return children; }
        }

        public void AddChildren(UnitAggregate unit)
        {
            if (unit.EndDate <= BeginDate || unit.BeginDate >= EndDate || unit.Deleted)
                return;
            children.Add(unit);
        }

        public double СalculationPeriod(DateTime period, DateTime beginDate, DateTime endDate)
        {
            if (endDate.Date <= BeginDate.Date || beginDate.Date >= EndDate.Date)
            {
                return 0.0;
            }
            var result = 0.0;
            foreach (var unit in children)
            {
                result += unit.СalculationPeriod(period, beginDate > BeginDate ? beginDate : beginDate, endDate < EndDate ? endDate : endDate);
            }
            return result;
        }
    }
}