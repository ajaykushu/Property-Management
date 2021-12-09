using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class CheckListQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [ForeignKey("CheckList")]
        public string CheckListId { get; set; }
        public CheckList CheckList{ get; set; }
        [ForeignKey("InspectionQueue")]
        public string InspectionQueueId { get; set; }
        public InspectionQueue InspectionQueue { get; set; }
        public int Status { get; set; }
    }
}
