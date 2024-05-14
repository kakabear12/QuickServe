using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class Product : BaseEntity
    {
        public Product()
        {
            IngredientProducts = new HashSet<ProductIngredient>();
            OrderProducts = new HashSet<OrderProduct>();
        }


        [Required]
        [StringLength(60)]
        public string ProductName { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public double Time { get; set; }

        [Required]
        public int ProductTemplateId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;

        [ForeignKey(nameof(ProductTemplateId))]
        public virtual ProductTemplate ProductTemplate { get; set; } = null!;

        public virtual ICollection<ProductIngredient> IngredientProducts { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
