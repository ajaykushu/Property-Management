using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Issue : Log
    {
        public Issue()
        {
            WorkOrders = new HashSet<WorkOrder>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string IssueName { get; set; }
        
       public int ItemId { get; set; }
        public Item Item { get; set; }

        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}