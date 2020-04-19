using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class WorkOrder : Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long PropertyId { get; set; }
        public Property Property { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int IssueId { get; set; }
        public Issue Issue { get; set; }
        public int StageId { get; set; }
        public Stage Stage { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Description { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string RequestedBy { get; set; }
        public long? AssignedToId { get; set; }
        [Column(TypeName = "varchar(300)")]
        public string AttachmentPath { get; set; }
        public DateTime DueDate { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public int? SubLocationId { get; set; }
        public SubLocation SubLocation { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}