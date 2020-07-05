using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Status : Log
    {
        public Status()
        {
            WorkOrders = new HashSet<WorkOrder>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string StatusDescription { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string StatusCode { get; set; }

        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}