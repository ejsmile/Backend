using Domain.Aggregates.Document;
using Domain.Const;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Aggregates.References
{
    public class MeterScaleAggregate : ICalcaulationPeriod
    {
        private readonly List<IndicationDocumentAggregate> indications = new List<IndicationDocumentAggregate>();

        public MeterScaleAggregate(long id, long ownerId, string name, ZoneOfDay zoneOfDayId, byte dimension, DateTime beginDate, DateTime endDate, bool isTransit, bool deleted)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            ZoneOfDayId = zoneOfDayId;
            BeginDate = beginDate;
            EndDate = endDate;
            IsTransit = isTransit;
            Dimension = dimension;
            Deleted = deleted;
        }

        public long Id { get; private set; }
        public long OwnerId { get; private set; }
        public string Name { get; private set; }
        public ZoneOfDay ZoneOfDayId { get; private set; }
        public byte Dimension { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsTransit { get; private set; }
        public bool Deleted { get; private set; }

        public long MaxIndication
            => (long)Math.Pow(10, Dimension);

        protected virtual void CheckIndication(double indication)
        {
            if (indication > MaxIndication)
            {
                throw new ArgumentException("Error indication");
            }
        }

        protected virtual double Consumption(double startIndication, double endIndication)
        {
            if (startIndication > endIndication)
            {
                return startIndication + MaxIndication - endIndication;
            }
            return endIndication - startIndication;
        }

        public void AddIndication(IndicationDocumentAggregate indication)
        {
            CheckIndication(indication.Indication);
            indications.Add(indication);
        }

        public double СalculationPeriod(DateTime period, DateTime beginDate, DateTime endDate)
        {
            if (indications.Count < 2)
                throw new ArgumentException("Error not set indication date");

            if (beginDate.Date >= endDate.Date)
                return 0;

            var startIndication = indications.Where(i => i.ReceiptDate <= beginDate).OrderByDescending(i => i.ReceiptDate).FirstOrDefault();
            var endIndication = indications.Where(i => i.ReceiptDate >= endDate).OrderBy(i => i.ReceiptDate).FirstOrDefault();
            if (endIndication == null)
            {
                endIndication = indications.Where(i => i.ReceiptDate > beginDate).OrderByDescending(i => i.ReceiptDate).FirstOrDefault();
            }

            if (startIndication == null || endIndication == null)
            {
                throw new ArgumentException("Error not find indication date");
            }

            //фактическое потребление
            if (beginDate.Date == startIndication.ReceiptDate.Date && endDate == endIndication.ReceiptDate.Date)
            {
                return Consumption(startIndication.Indication, endIndication.Indication);
            }
            //прогноз по потреблению
            var diffPeriod = (endIndication.ReceiptDate.Date - startIndication.ReceiptDate.Date).TotalDays;
            if (diffPeriod > 45)
                throw new ArgumentException("Error indication date big period");
            var velocity = Consumption(startIndication.Indication, endIndication.Indication) / diffPeriod;

            return (endDate.Date - beginDate.Date).TotalDays * velocity;
        }
    }
}