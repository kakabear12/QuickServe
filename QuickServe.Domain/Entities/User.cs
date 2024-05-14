using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Domain.Entities
{
    public partial class User : BaseEntity
    {
        public User()
        {
            FeedBacks = new HashSet<FeedBack>();
            Orders = new HashSet<Order>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        [Required]
        [StringLength(40, MinimumLength = 6)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string FullName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Phone]
        [StringLength(11)]
        public string Phone { get; set; } = null!;

        public string? Avatar { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = null!;

        public DateTime? Birthday { get; set; }
        [StringLength(200, MinimumLength = 4)]
        public string? Address { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;

        public string? ConfirmationToken { get; set; }

        [Required]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<FeedBack> FeedBacks { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
