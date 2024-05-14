using QuickServe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Domain.Entities
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>(); 
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [StringLength(20)]
        public string RoleName { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
