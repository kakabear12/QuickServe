using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class ProductIngredient
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int IngredientId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(IngredientId))]
        public virtual Ingredient Ingredient { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;

    }
}