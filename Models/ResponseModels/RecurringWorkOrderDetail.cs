using System;
using System.Collections.Generic;

namespace Models.ResponseModels
{
    public class RecurringWorkOrderDetail
    {
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string SubLocation { get; set; }
        public string Location { get; set; }
        public string Requestedby { get; set; }
        public string StatusDescription { get; set; }
        public string Description { get; set; }
        public string Issue { get; set; }
        public string Item { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public int Priority { get; set; }
        public DateTime DueDate { get; set; }
        public List<KeyValuePair<string, string>> Attachment { get; set; }
        public string UpdatedBy { get; set; }
        public string AssignedTo { get; set; }
        
        public string Vendor { get; set; }
        public bool Recurring { get; set; } = true;
        public DateTime? RecurringEndDate { get; set; }
        public int? EndAfterCount { get; set; }
        public string CronExpression { get; set; }
        public DateTime? RecurringStartDate { get; set; }
    }
}