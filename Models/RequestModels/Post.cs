using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.RequestModels
{
    public class Post
    {   
        [Required]
        public string Comment { get; set; }
        [Required]
        public long WorkOrderId { get; set; }
        public long ParentId { get; set; }
        public string RepliedTo { get; set; }
    }
}
