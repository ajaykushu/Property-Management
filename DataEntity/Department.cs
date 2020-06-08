using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Department : Log
    {
        public Department()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string DepartmentName { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}