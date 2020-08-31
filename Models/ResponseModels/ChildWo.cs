using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ResponseModels
{
    public class ChildWo
    {
        public string Id { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; }
        public SelectItem Property { get; set; }
    }
}
