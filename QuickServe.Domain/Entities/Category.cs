using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            ProductTemplates = new HashSet<ProductTemplate>();
        }

   
        [Required]
        [StringLength(40)]
        public string CategoryName { get; set; } = null!;
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;

        public virtual ICollection<ProductTemplate> ProductTemplates { get; set; }
    }
}
