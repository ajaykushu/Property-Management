using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Item : Log
    {
        public Item()
        {
            WorkOrders = new HashSet<WorkOrder>();
            Issues = new HashSet<Issue>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ItemName { get; set; }

        public  bool Active { get; set; }
       
        public Location Location { get; set; }
        public int? LocationId { get; set; }

        public ICollection<Issue> Issues { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}