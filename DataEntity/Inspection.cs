using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class Inspection : Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Type")]
        public int Type { get; set; } //1 for inspection and 2 for checklist
        [ForeignKey("Department")]
        public  int DepartmentID { get; set; }
        public Department Department { get; set; }
        public  ICollection<CheckList> ChecklistItems { get; set; }
        public string CronExpression { get; set; }
        public Property Property { get; set; }
        [ForeignKey("Property")]
        public long PropertyId { get; set; }
        public bool Active { get; set; }
        public ICollection<InspectionQueue> InspectionQueues { get; set; }
    }
}
