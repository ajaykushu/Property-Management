using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class EditWorkOrder
    {
       
        public long Id { get; set; }
        public List<SelectItem> Locations { get; set; }
        [Required]
        [DisplayName("Location Id")]
        public int LocationId { get; set; }
        public List<SelectItem> SubLocations { get; set; }
        [Required]
        [DisplayName("Sub Location")]
        public int SubLocationId { get; set; }
        public List<SelectItem> Items { get; set; }

        [Required(ErrorMessage = "Please Select Item")]
        [DisplayName("Item")]
        public int ItemId { get; set; }

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
        [Range(0,3)]
        public int Priority { get; set; }

        [Required(ErrorMessage = "Please give Department Id")]
        [DisplayName("Department")]
        public int DepartmentId { get; set; }
        public List<SelectItem> Users { get; set; }
        [Required(ErrorMessage = "Please give User")]
        [DisplayName("User")]
        public long UserId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MMM/yyyy}")]
        public DateTime DueDate { get; set; }
        public List<SelectItem> Sections { get; set; }
        public IList<IFormFile> File { get; set; }
    }
}