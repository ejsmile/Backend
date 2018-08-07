using System;

namespace ApiContracts.References.Agreement
{
    public class AgreementView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Deleted { get; set; }
    }

    public class AgreementCalculationPeriodView
    {
        public long Id { get; set; }
        public DateTime Period { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Consumption { get; set; }
    }
}