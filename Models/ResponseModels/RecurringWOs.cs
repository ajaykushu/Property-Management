using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjects.ResponseModels
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
