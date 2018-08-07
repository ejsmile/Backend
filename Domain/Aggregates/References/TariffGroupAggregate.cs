using System;
using System.Collections.Generic;

namespace Domain.Aggregates.References
{
    public class TariffGroupAggregate : ICalcaulationPeriod
    {
        private readonly List<ConstantFlowAggregate> childredsConstantFlow = new List<ConstantFlowAggregate>();
        private readonly List<ChannelAggregate> childredsChannel = new List<ChannelAggregate>();

        public TariffGroupAggregate(long ownerId, long id, string name, string tariffName, double price, DateTime begDate, DateTime endDate, bool isTransit, bool deleted)
        {
            OwnerId = ownerId;
            Id = id;
            Name = name;
            TariffName = tariffName;
            Price = price;
            BeginDate = begDate;
            EndDate = endDate;
            IsTransit = isTransit;
            Deleted = deleted;
        }

        public long OwnerId { get; private set; }
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string TariffName { get; private set; }
        public double Price { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsTransit { get; private set; }
        public bool Deleted { get; private set; }

        public void AddChildrenConst(ConstantFlowAggregate constFlow)
        {
            if (constFlow.EndDate <= BeginDate || constFlow.BeginDate >= EndDate || constFlow.Deleted)
                return;
            childredsConstantFlow.Add(constFlow);
        }

        public void AddChildrenChannel(ChannelAggregate channel)
        {
            if (channel.EndDate <= BeginDate || channel.BeginDate >= EndDate || channel.Deleted)
                return;
            childredsChannel.Add(channel);
        }

        public double СalculationPeriod(DateTime period, DateTime beginDate, DateTime endDate)
        {
            if (endDate.Date <= BeginDate.Date || beginDate.Date >= EndDate.Date)
            {
                return 0.0;
            }
            var result = 0.0;
            foreach (var children in childredsConstantFlow)
            {
                result += (children.IsTransit ? -1 : 1)
                    * children.СalculationPeriod(period, beginDate > BeginDate ? beginDate : beginDate, endDate < EndDate ? endDate : endDate);
            }

            foreach (var children in childredsChannel)
            {
                result += (children.IsTransit ? -1 : 1)
                    * children.СalculationPeriod(period, beginDate > BeginDate ? beginDate : beginDate, endDate < EndDate ? endDate : endDate);
            }
            return result;
        }
    }
}