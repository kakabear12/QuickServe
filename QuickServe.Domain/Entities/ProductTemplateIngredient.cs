using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Domain.Entities
{
    public partial class ProductTemplateIngredient
    {
        [Key]
        [Column(Order = 1)]
        public int ProductTemplateId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int IngredientId { get; set; }

        public int? Quantity { get; set; }

        [ForeignKey(nameof(ProductTemplateId))]
        public virtual ProductTemplate ProductTemplate { get; set; } = null!;

        [ForeignKey(nameof(IngredientId))]
        public virtual Ingredient Ingredient { get; set; }= null!;

    }
}
