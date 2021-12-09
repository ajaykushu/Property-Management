using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class Inspection
    {
        
        public string Id { get; set; }
        [Required(ErrorMessage ="Please Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Choose Type")]
        [DisplayName("Insp/Checklist")]
        public int Type { get; set; } //1 for inspection and 2 for checklist

        [Required(ErrorMessage = "Please Choose Department")]
        [DisplayName("Used By")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage ="Please Choose Recurring")]
        [DisplayName("Recurring")]
        public string CronExpression { get; set; }
        public int Status { get; set; }//1 in progress 2 complete  3 pause
        public List<SelectItem> Departments { get; set; }
        public List<SelectItem> Properties { get; set; }
        [DisplayName("This inspection template is visible to highlighted properties below")]
        public long PropertyId { get; set; }

    }
}
