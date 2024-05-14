using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServe.Domain.Entities
{
    public partial class FeedBack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [StringLength(200)]
        public string? Comment { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

    }
}
