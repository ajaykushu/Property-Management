using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Issue
    {
        public Issue()
        {
            WorkOrders = new HashSet<WorkOrder>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string IssueName { get; set; }

        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}