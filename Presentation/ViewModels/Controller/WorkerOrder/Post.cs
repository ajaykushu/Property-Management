namespace Presentation.ViewModels
{
    public class Post
    {
        public string Comment { get; set; }
        public string WorkOrderId { get; set; }
        public long ParentId { get; set; }
        public string RepliedTo { get; set; }
    }
}