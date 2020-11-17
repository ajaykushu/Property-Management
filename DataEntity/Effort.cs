using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class Effort:Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int TaxBO { get; set; }
        public int Repair { get; set; }
        [ForeignKey("WorkOrder")]
        public string WOId { get; set; }
        public WorkOrder WorkOrder {get;set;}
    }
}
