using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.WorkOrder.RequestModels
{
    public class CreateNormalWO
    {
        public List<SelectItem> Locations { get; set; }

        [Required]
        public int LocationId { get; set; }

        public List<SelectItem> SubLocations { get; set; }

        [Required]
        public int SubLocationId { get; set; }

       //public List<SelectItem> Items { get; set; }

        [Required(ErrorMessage = "Please Select Item")]
        public int ItemId { get; set; }

        public List<SelectItem> Properties { get; set; }

        [Required(ErrorMessage = "Please Give Property Id")]
        public long PropertyId { get; set; }

        public List<SelectItem> Issues { get; set; }

        [Required(ErrorMessage = "Please Select Issue Id")]
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }
        public string Category { get; set; }
        public List<SelectItem> Options { get; set; }
        public int? OptionId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int Priority { get; set; }

        public int? VendorId { get; set; }
        public List<SelectItem> Vendors { get; set; }
        public string CustomIssue { get; set; }

        
    }
}