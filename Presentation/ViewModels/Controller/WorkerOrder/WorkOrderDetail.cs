using Presentation.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Presentation.ViewModels
{
    public class WorkOrderDetail
    {
        public string Id { get; set; }

        [DisplayName("Property Name")]
        public string PropertyName { get; set; }

        [DisplayName("Sub Location")]
        public string SubLocation { get; set; }

        public string Location { get; set; }

        [DisplayName("Requested By")]
        public string Requestedby { get; set; }


        [DisplayName("Status Description")]
        public string StageDescription { get; set; }

        [DisplayName("Status Code")]
        public string StageCode { get; set; }

        public string Issue { get; set; }

        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }

        public string Item { get; set; }

        [DisplayName("Created Time")]
        public DateTime CreatedTime { get; set; }

        public string Description { get; set; }
        public int Priority { get; set; }

        [DisplayName("Updated Time")]
        public DateTime UpdatedTime { get; set; }

        public List<KeyValuePair<string, string>> Attachment { get; set; }

        [DisplayName("Assigned To")]
        public string AssignedTo { get; set; }

        [DisplayName("Updated By")]
        public string UpdatedBy { get; set; }

        [SkipProperty]
        public List<SelectItem> Stages { get; set; }

        [SkipProperty]
        public Pagination<List<Comment>> Comments { get; set; }
    }
}