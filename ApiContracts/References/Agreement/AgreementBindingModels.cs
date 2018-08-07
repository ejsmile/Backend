using System;
using System.ComponentModel.DataAnnotations;

namespace ApiContracts.References.Agreement
{
    public class AgreementCreateBindingModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Number { get; set; }

        [Required]
        public DateTime BegDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool Deleted { get; set; }
    }

    public class AgreementCalculationPeriodBindingModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public DateTime Period { get; set; }

        [Required]
        public DateTime BeginDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}