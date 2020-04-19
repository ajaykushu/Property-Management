using System;
using System.Collections.Generic;

namespace Models.ResponseModels
{
    public class WorkOrderDetail
    {
        public long Id { get; set; }
        public string PropertyName { get; set; }
        public string SubLocation { get; set; }
        public string Location { get; set; }
        public string Requestedby { get; set; }
        public string Department { get; set; }
        public string StageDescription { get; set; }
        public string Description { get; set; }
        public string StageCode { get; set; }
        public string Issue { get; set; }
        public string Item { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime DueDate { get; set; }
        public string Attachment { get; set; }
        public string UpdatedBy { get; set; }
        public string AssignedToUser { get; set; }
        public List<SelectItem> Stages { get; set; }
    }
}