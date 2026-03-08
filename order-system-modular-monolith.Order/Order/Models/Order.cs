using order_system_modular_monolith.BuildingBlocks.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_system_modular_monolith.Order.Models
{
    [Table("orders", Schema = "orders")]
    public class Orders : FullTrackedAggregateRoot<Guid>
    {
        [Key]
        [Column("id")]
        public override Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        [Column("orderNumber")]
        public string OrderNumber { get; set; } = default!;
        [Required]
        [Column("total")]
        public decimal Total { get; set; }

        public Orders(Guid id) : base(id)
        {
        }
       public Orders()
        {
        }
    }
}
