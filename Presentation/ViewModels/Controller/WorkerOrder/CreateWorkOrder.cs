using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Presentation.ViewModels
{
    public class CreateWorkOrder
    {
        public List<SelectItem> Locations { set; get; }
        [Required(ErrorMessage = "Please Select Location")]
        public int Location { set; get; }
        public List<SelectItem> Areas { get; set; }
        [Required(ErrorMessage = "Please Select Area")]
        public int Area { get; set; }
        public List<SelectItem> Items { get; set; }
        [Required(ErrorMessage = "Please Select Item")]
        public int Item { get; set; }
        public List<SelectItem> Issues { get; set; }
        [Required(ErrorMessage = "Please Select Issue")]
        public int Issue { get; set; }
        public List<SelectItem> RequestedBy { get; set; }
        [Required(ErrorMessage = "Please Select Requested By")]
        public int RequestedById { get; set; }
        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Details { set; get; }
        [Required(ErrorMessage = "Please Select Date")]
        public DateTime Date { get; set; }
        public List<SelectItem> Departments { get; set; }
        [Required(ErrorMessage = "Please Select Department")]
        public int Department { get; set; }
        public List<SelectItem> Enginnerings { get; set; }
        [Required(ErrorMessage = "Please Select Section")]
        public int Enginnering { get; set; }
        [Required(ErrorMessage = "Please Select Photo")]
        public IFormFile Photos { set; get; }

    }
}
