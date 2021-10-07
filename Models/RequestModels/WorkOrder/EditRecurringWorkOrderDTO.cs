using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.WorkOrder.RequestModels
{
    public class EditRecurringWorkOrderDTO
    {
        public string Id { get; set; }
        public List<SelectItem> Locations { get; set; }

        [Required]
        public int LocationId { get; set; }

        public List<SelectItem> SubLocations { get; set; }

        [Required]
        public int SubLocationId { get; set; }

        public List<SelectItem> Items { get; set; }

        
        public int? ItemId { get; set; }

        public string PropertyName { get; set; }

        public List<SelectItem> Issues { get; set; }

        [Required(ErrorMessage = "Please Select Issue Id")]
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Please give Date")]
        public DateTime CreatedDate { get; set; }

        public List<KeyValuePair<string, string>> FileAvailable { get; set; }
        public string Category { get; set; }
        public Dictionary<string, List<SelectItem>> Options { get; set; }
        public int? OptionId { get; set; }

        public uint DueAfterDays { get; set; }
        public List<SelectItem> Users { get; set; }
        public string FilesRemoved { get; set; }
        public int Priority { get; set; }
        public int? VendorId { get; set; }
        public List<SelectItem> Vendors { get; set; }
        [Required]
        public string CronExpression { get; set; }
        public DateTime? RecurringEndDate { get; set; }
        public int? EndAfterCount { get; set; }
        public DateTime? RecurringStartDate { get; set; }
        public string CustomIssue { get; set; }
    }
}