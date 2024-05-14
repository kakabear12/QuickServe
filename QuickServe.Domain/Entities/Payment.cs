using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string PaymentType { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
