using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Domain.Entities
{
    public class RefreshToken
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [StringLength(1000)]
        [Required]
        public string TokenHash { get; set; } = null!;
        [StringLength(50)]
        public string TokenSalt { get; set; } = null!;
        [Required]
        public DateTime Ts { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}
