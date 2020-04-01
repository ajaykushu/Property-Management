using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Stage
    {
        public Stage()
        {
            workOrders = new HashSet<WorkOrder>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string StageDescription { get; set; }

        [Column(TypeName = "varchar(7)")]
        public string StageCode { get; set; }

        public virtual ICollection<WorkOrder> workOrders { get; set; }
    }
}