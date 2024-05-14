using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class OrderProduct
    {
        [Key, Column(Order = 1)]
        public int OrderId { get; set; }

        [Key, Column(Order = 2)]
        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}