using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.Module.Stock.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_system_modular_monolith.Stock.Models
{
    [Table("stocks", Schema = "stocks")]
    public class Stocks : FullTrackedAggregateRoot<Guid>
    {
        [Key]
        [Column("id")]
        public override Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("productCode")]
        public string ProductCode { get; set; } = default!;

        [Required]
        [MaxLength(11)]
        [Column("quantity")]
        public decimal Quantity { get; set; } = default!;

        public Stocks(Guid id) : base(id)
        {
        }

        public Stocks()
        {

        }

    }
}
