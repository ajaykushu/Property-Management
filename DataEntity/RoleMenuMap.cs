using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class RoleMenuMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public long RoleId { get; set; }
        [Required]
        public long MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}