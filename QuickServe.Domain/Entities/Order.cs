using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class Order : BaseEntity
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

  

        [Required]
        public int PaymentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public double TotalTime { get; set; }

        [Required]
        public int TableNumber { get; set; }

        [Required]
        public int StoreId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(PaymentId))]
        public virtual Payment Payment { get; set; } = null!;

        [ForeignKey(nameof(StoreId))]
        public virtual Store Store { get; set; } = null!;

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
