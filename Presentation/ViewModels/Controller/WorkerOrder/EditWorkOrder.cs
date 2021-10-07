using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class EditWorkOrder
    {
        public string Id { get; set; }
        public List<SelectItem> Locations { get; set; }

        [Required]
        [DisplayName("Location Id")]
        public int LocationId { get; set; }

        public int? VendorId { get; set; }
        public List<SelectItem> Vendors { get; set; }
        public List<SelectItem> SubLocations { get; set; }

        [Required]
        [DisplayName("Sub Location")]
        public int SubLocationId { get; set; }

        public List<SelectItem> Items { get; set; }

        
        [DisplayName("Item")]
        public int? ItemId { get; set; }

        public string PropertyName { get; set; }

        public List<SelectItem> Issues { get; set; }

        [DisplayName("Issue")]
       

        [Required(ErrorMessage = "Please Select Issue Id")]
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Please give Date")]
        public DateTime CreatedDate { get; set; }

        public List<SelectItem> Departments { get; set; }
        public List<KeyValuePair<string, string>> FileAvailable { get; set; }
        public string FilesRemoved { get; set; }

        [Required]
        [Range(0, 3)]
        public int Priority { get; set; }
        [Required(ErrorMessage = "Please Select Assign to")]
        public string Category { get; set; }
        public Dictionary<string, List<SelectItem>> Options { get; set; }
        [DisplayName("Option")]
        public int? OptionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MMM/yyyy}")]
        
        public DateTime DueDate { get; set; }

        public List<SelectItem> Sections { get; set; }
        public IList<IFormFile> File { get; set; }
        [StringLength(100, ErrorMessage = "Limit to 100 characters")]
        public string  CustomIssue { get; set; }
    }
}