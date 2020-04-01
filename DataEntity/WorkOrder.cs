using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class WorkOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
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

        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}