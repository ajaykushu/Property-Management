using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AreaName { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
