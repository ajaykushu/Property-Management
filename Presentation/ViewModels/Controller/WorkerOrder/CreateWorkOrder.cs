using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    [Serializable]
    public class CreateWorkOrder
    {
        [DisplayName("Location")]
        public int LocationId { get; set; }
        public List<SelectItem> Locations { get; set; }
        [Required]
        [DisplayName("Sub Location")]
        public int SubLocationId { get; set; }
        public List<SelectItem> SubLocation { get; set; }
        public List<SelectItem> Items { get; set; }
        [DisplayName("Item")]
        [Required(ErrorMessage = "Please Select Item")]
        public int ItemId { get; set; }

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

        public List<SelectItem> Departments { get; set; }

        [Required(ErrorMessage = "Please Select Department")]
        [DisplayName("Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Select Section")]
        [DisplayName("User")]
        public int UserId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:MM/dd/yyyy}")]
        public DateTime DueDate { get; set; }
        public IFormFile File { get; set; }
    }
}