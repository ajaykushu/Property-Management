using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Item
    {
        public Item()
        {
            workOrders = new HashSet<WorkOrder>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string ItemName { get; set; }

        public virtual ICollection<WorkOrder> workOrders { get; set; }
    }
}