using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Presentation.ViewModels
{
    public class CreateWorkOrder
    {
        public string Location { get; set; }
        public string Area { get; set; }
        public List<SelectItem> Items { get; set; }
        [Required(ErrorMessage = "Please Select Item")]
        public int Item { get; set; }
        public List<SelectItem> Properties { get; set; }
        [Required(ErrorMessage = "Please Select Item")]
        public long Property { get; set; }
        public List<SelectItem> Issues { get; set; }
        [Required(ErrorMessage = "Please Select Issue")]
        public int Issue { get; set; }
        public List<SelectItem> RequestedBy { get; set; }
        [Required(ErrorMessage = "Please Select Requested By")]
        public int RequestedById { get; set; }
        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }
        [Required(ErrorMessage = "Please Select Date")]
        public DateTime Date { get; set; }
        public List<SelectItem> Departments { get; set; }
        [Required(ErrorMessage = "Please Select Department")]
        public int Department { get; set; }
        public List<SelectItem> Sections { get; set; }
        [Required(ErrorMessage = "Please Select Section")]
        public int Section { get; set; }
        public IFormFile Photos { set; get; }

    }
}
