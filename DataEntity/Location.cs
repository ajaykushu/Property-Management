using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LocationName { get; set; }

        public long PropertyId { get; set; }
        public Property Property { get; set; }
        public virtual ICollection<SubLocation> SubLocations { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
    }
}