using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class SubLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string AreaName { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}