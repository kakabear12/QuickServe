using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class Ingredient : BaseEntity
    {
        public Ingredient()
        {
            IngredientProducts = new HashSet<ProductIngredient>();
        }

 

        [Required]
        [StringLength(60)]
        public string IngredientName { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Calo { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public int IngredientTypeId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;

        [ForeignKey(nameof(IngredientTypeId))]
        public virtual IngredientType IngredientType { get; set; } = null!;

        public virtual Nutrition? Nutrition { get; set; }

        public virtual ICollection<ProductIngredient> IngredientProducts { get; set; }

    }
}
