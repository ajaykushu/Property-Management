using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public long WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Comment { get; set; }

        public Comments Parent { get; set; }
        public long? ParentId { get; set; }
    }
}