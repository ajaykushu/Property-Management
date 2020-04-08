using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class Comment
    {
        public long Id { get; set; }
        public string CommentString { get; set; }
        public string CommentDate { get; set; }
        public string CommentBy { get; set; }
        public List<Reply> Reply { get; set; }
    }

    public class Reply
    {
        public long Id { get; set; }
        public string ReplyString { get; set; }
        public string RepliedTo { get; set; }
        public string RepliedDate { get; set; }
        public string RepliedBy { get; set; }
    }
}