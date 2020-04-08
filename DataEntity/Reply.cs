using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Reply : Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long CommentId { get; set; }
        public Comments Comments { get; set; }
        public string AttachmentPath { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string ReplyString { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string RepliedTo { get; set; }
    }
}