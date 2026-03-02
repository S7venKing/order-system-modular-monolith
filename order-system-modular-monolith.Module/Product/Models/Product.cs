using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.Module.Product.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_system_modular_monolith.Product.Models
{
    [Table("products", Schema = "products")]
    public class Products : FullTrackedAggregateRoot<Guid>
    {
        [Key]
        [Column("id")]
        public override Guid Id { get ; set ; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("price")]
        public decimal Price { get; set; }

        [Column("category")]
        public string Category { get; set; }

        public Products(Guid id) : base(id)
        {
        }

    }
}
