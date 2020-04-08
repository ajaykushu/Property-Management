using System.Collections.Generic;

namespace Models.ResponseModels
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public string CommentString { get; set; }
        public string CommentDate { get; set; }
        public string CommentBy { get; set; }
        public List<ReplyDTO> Reply { get; set; }
    }

    public class ReplyDTO
    {
        public long Id { get; set; }
        public string ReplyString { get; set; }
        public string RepliedTo { get; set; }
        public string RepliedDate { get; set; }
        public string RepliedBy { get; set; }
    }
}