using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    [Serializable]
    public class CreateWorkOrderRecurring
    {
        [DisplayName("Location")]
        public int LocationId { get; set; }

        public List<SelectItem> Locations { get; set; }

        [Required]
        [DisplayName("Sub Location")]
        public int SubLocationId { get; set; }

        public List<SelectItem> SubLocation { get; set; }
        //public List<SelectItem> Items { get; set; }

        [DisplayName("Item")]
       
        public int? ItemId { get; set; }

        public int? VendorId { get; set; }
        public List<SelectItem> Vendors { get; set; }
        public List<SelectItem> Properties { get; set; }

        [Required(ErrorMessage = "Please Select Property")]
        [DisplayName("Property")]
        public long PropertyId { get; set; }

        public List<SelectItem> Issues { get; set; }

        [DisplayName("Issue")]
        [Required(ErrorMessage = "Please Select Issue")]
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }
        [Required(ErrorMessage = "Please Select Assign to")]
        public string Category { get; set; }
        public List<SelectItem> Options { get; set; }
        [DisplayName("Option")]
        public int? OptionId { get; set; }

        [Required]
        [Range(0, 366)]
        public uint DueAfterDays { get; set; }

        public IList<IFormFile> File { get; set; }

        [Required]
        [Range(0, 3)]
        public int Priority { get; set; }
        [Required(ErrorMessage ="Please Enter Schedule Details")]
        public string CronExpression { get; set; }
        public DateTime? RecurringEndDate { get; set; }
        public int? EndAfterCount { get; set; }
        public DateTime? RecurringStartDate { get; set; }
        [StringLength(100, ErrorMessage = "Limit to 100 characters")]
        public string CustomIssue { get; set; }
    }
}
