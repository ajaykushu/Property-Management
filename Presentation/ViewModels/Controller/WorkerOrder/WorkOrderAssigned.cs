using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.ViewModels
{
    public class WorkOrderAssigned
    {
        public long Id { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
    }
}
