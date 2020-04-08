using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class WorkOrderDetail
    {
        public long Id { get; set; }
        public string PropertyName { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public string Requestedby { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string StageDescription { get; set; }
        public string StageCode { get; set; }
        public string Issue { get; set; }
        public string Item { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string AssignedToUser { get; set; }
        public string UpdatedBy { get; set; }
        public List<SelectItem> Users { get; set; }
    }
}