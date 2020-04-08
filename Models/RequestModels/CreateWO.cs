using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
   
    public class CreateWO
    {
        public string Location { get; set; }
        public string Area { get; set; }
        public List<SelectItem> Items { get; set; }

        [Required(ErrorMessage = "Please Select Item")]
        public int Item { get; set; }

        public List<SelectItem> Properties { get; set; }

        [Required(ErrorMessage = "Please Give Property Id")]
        public long Property { get; set; }

        public List<SelectItem> Issues { get; set; }

        [Required(ErrorMessage = "Please Select Issue Id")]
        public int Issue { get; set; }

        [Required(ErrorMessage = "Please Give Some Detail")]
        public string Description { set; get; }

        public List<SelectItem> Departments { get; set; }

        [Required(ErrorMessage = "Please give Department Id")]
        public int Department { get; set; }

        [Required(ErrorMessage = "Please give Section Id")]
        public int Section { get; set; }
        public IFormFile File { get; set; }
    }
}