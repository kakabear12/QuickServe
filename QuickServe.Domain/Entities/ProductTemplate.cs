using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class ProductTemplate : BaseEntity
    {
        public ProductTemplate()
        {
            Products = new HashSet<Product>();
        }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public int? Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public string Size { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StoreId { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        [ForeignKey(nameof(StoreId))]
        public virtual Store Store { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } 
    }
}