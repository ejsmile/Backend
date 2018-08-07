using System;

namespace Domain.Aggregates.References
{
    public interface ICalcaulationPeriod
    {
        double СalculationPeriod(DateTime period, DateTime beginDate, DateTime endDate);
    }
}