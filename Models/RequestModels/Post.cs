using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
    public class Post
    {
        [Required]
        public string Comment { get; set; }

        [Required]
        public string WorkOrderId { get; set; }

        public long ParentId { get; set; }
        public string RepliedTo { get; set; }
    }
}