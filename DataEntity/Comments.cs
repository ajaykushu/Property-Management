using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Comment : Log
    {
        public Comment()
        {
            Replies = new HashSet<Reply>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string WorkOrderId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string CommentString { get; set; }

        [ForeignKey("ApplicationUser")]
        public long CommentById { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Reply> Replies { get; set; }
    }
}