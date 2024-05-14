using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 6)]
        public string CreatedBy { get; set; } = null!;
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 6)]
        public string UpdatedBy { get; set; } = null!;
    }
}
