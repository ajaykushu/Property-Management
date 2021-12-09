using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class CheckList : Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [ForeignKey("Checklist")]
        public string InspectionId { get; set; }
        public Inspection Inspection { get; set; }
        public string Description { get; set; }
        [ForeignKey("SubLocation")]
        public int? SubLocationId {get;set;}
        public SubLocation SubLocation { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int LocationOrder { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }
    }
}
