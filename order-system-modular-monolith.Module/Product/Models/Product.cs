using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.Module.Product.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_system_modular_monolith.Module.Product.Models
{
    [Table("products", Schema = "products")]
    public class Product : FullTrackedAggregateRoot<Guid>
    {
        [Key]
        [Column("id")]
        public override Guid Id { get ; set ; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; private set; } = default!;

        public Product(Guid id) : base(id)
        {
        }

    }
}
