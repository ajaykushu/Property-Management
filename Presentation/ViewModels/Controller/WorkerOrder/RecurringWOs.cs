using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class RecurringWOs
    {
        public string Id { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public string ScheduleAt { get; set; }

        public string DueAfterDays { get; set; }
        public string Status { get; set; }
        public SelectItem Property { get; set; }
    }
}
