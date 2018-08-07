using System.ComponentModel.DataAnnotations;

namespace ApiContracts.References.Agreement
{
    public class TariffCreateBindingModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public bool Deleted { get; set; }
    }
}