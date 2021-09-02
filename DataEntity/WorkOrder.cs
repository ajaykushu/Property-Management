using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class WorkOrder : Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Id { get; set; }
        public long PropertyId { get; set; }
        public Property Property { get; set; }
        public int? ItemId { get; set; }
        public string  CustomIssue { get; set; }
        public Item Item { get; set; }
        public int? IssueId { get; set; }
        public Issue Issue { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Description { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string RequestedBy { get; set; }
        public long? AssignedToId { get; set; }
        public int? AssignedToDeptId { get; set; }
        public int? VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public DateTime DueDate { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public int? SubLocationId { get; set; }
        public SubLocation SubLocation { get; set; }
        [ForeignKey("ParentRecurringWO")]
        public string ParentWoId { set; get; }
        public RecurringWO ParentRecurringWO { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public long TotalEffort { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }
        public virtual Department AssignedToDept { get; set; }
        public virtual ICollection<WOAttachments> WOAttachments { get; set; }
        public virtual ICollection<Effort> Efforts { get; set; }
    }
}