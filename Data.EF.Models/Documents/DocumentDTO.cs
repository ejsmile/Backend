using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EF.Models.Documents
{
    [Table("Documents")]
    public abstract class DocumentDTO : IDeleted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Deleted { get; set; }
    }
}