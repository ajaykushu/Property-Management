using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Comments : Log
    {
        public Comments()
        {
            Replies = new HashSet<Reply>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Comment { get; set; }
        public string AttachmentPath { get; set; }
        public ICollection<Reply> Replies { get; set; }

    }
}