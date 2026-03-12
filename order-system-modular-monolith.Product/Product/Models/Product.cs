using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.Module.Product.ValueObjects;
using order_system_modular_monolith.Product.Domain;
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
        [Column("code")]
        public string Code { get; set; } = default!;

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

        public Products(Guid id, string productCode, long quantity) : base(id)
        {
            Code = productCode;

            Raise(new ProductCreatedDomainEvent(id, productCode, quantity));
        }

        public Products()
        {

        }

    }
}
