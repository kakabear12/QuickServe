using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class Nutrition : BaseEntity
    {
     
        [Required]
        public int IngredientId { get; set; }

        public string? ImageUrl { get; set; }
        [StringLength(40)]
        public string? Name { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        [StringLength(40)]
        public string? Vitamin { get; set; }
        [StringLength(60)]
        public string? HealthValue { get; set; }

        [ForeignKey(nameof(IngredientId))]
        public virtual Ingredient? Ingredient { get; set; }
    }
}
