using System.ComponentModel.DataAnnotations;

namespace Models.WorkOrder.RequestModels
{
    public class PostCommentDTO
    {
        [Required]
        public string Comment { get; set; }

        [Required]
        public string WorkOrderId { get; set; }

        public long ParentId { get; set; }
        public string RepliedTo { get; set; }
    }
}