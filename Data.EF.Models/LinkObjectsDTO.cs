using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models
{
    [Table("Links")]
    public class LinkObjectsDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public virtual ObjectDTO Owner { get; set; }

        [Required]
        public virtual ObjectDTO Childen { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public double Factore { get; set; }

        public bool IsTransit { get; set; }

        public bool Deleded { get; set; }
    }
}