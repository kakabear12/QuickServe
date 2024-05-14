using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class IngredientType : BaseEntity
    {
        public IngredientType()
        {
            Ingredients = new HashSet<Ingredient>();
        }


        [Required]
        [StringLength(40)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
