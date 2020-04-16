using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class EditWorkOrder
    {
       
        public long Id { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
        public List<SelectItem> Items { get; set; }

        [Required(ErrorMessage = "Please Select Item")]
        public int Item { get; set; }

        public string PropertyName { get; set; }

        public List<SelectItem> Issues { get; set; }

        [Required(ErrorMessage = "Please Select Issue Id")]
        public int Issue { get; set; }

        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }

        [Required(ErrorMessage = "Please give Date")]
        public DateTime CreatedDate { get; set; }

        public List<SelectItem> Departments { get; set; }

        [Required(ErrorMessage = "Please give Department Id")]
        public int Department { get; set; }

        [Required(ErrorMessage = "Please give Section Id")]
        public long Section { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MMM/yyyy}")]
        public DateTime DueDate { get; set; }
        public List<SelectItem> Sections { get; set; }
        public IFormFile File { get; set; }
    }
}