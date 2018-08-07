using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.References
{
    [Table("Agreement")]
    public class AgreementDTO : ObjectDTO
    {
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }

        [MaxLength(127)]
        public string Number { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }
    }
}