using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Domain.Entities
{
    public partial class Store : BaseEntity
    {
        public Store()
        {
            Orders = new HashSet<Order>();
            ProductTemplates = new HashSet<ProductTemplate>();
        }

        [Required]
        [StringLength(100)]
        public string StoreName { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<ProductTemplate> ProductTemplates { get; set; }

    }
}
