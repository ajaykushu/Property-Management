using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Department
    {
        public Department()
        {
            this.UserType = new HashSet<ApplicationRole>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(30)")]
        public string DepartmentName { get; set; }
        public virtual ICollection<ApplicationRole> UserType { get; set; }

    }
}
