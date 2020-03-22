using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class WorkerType
    {
        public WorkerType()
        {
            this.WorkerTyperUserMaps = new HashSet<WorkerTyperUserMap>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(30)")]
        public string TypeName { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public virtual Department Departments { get; set; }
        [Required]
        public int PayScaleId { get; set; }
        public virtual PayScale PayScale { get; set; }
        public virtual ICollection<WorkerTyperUserMap> WorkerTyperUserMaps { set; get; }
    }
}