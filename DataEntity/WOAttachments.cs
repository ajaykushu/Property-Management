using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class WOAttachments : Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Key { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string FileName { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string FilePath { get; set; }

        public string WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }
    }
}