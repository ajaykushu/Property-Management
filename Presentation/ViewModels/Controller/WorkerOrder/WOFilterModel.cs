using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class WOFilterModel
    {
        [DisplayName("Assigned To")]
        [RegularExpression(@"[a-z0-9 ]*",ErrorMessage ="only lower case allowed")]
        public string AssignedTo { get; set; }
        [RegularExpression(@"[a-z0-9 \W]*", ErrorMessage = "only lower case allowed")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Property Name")]
        [RegularExpression(@"[a-z0-9 ]*", ErrorMessage = "only lower case allowed")]
        public string PropertyName { get; set; }
        [DisplayName("Creation Start Date")]
        public string CreationStartDate { get; set; }
        [DisplayName("Creation End Date")]
        public string CreationEndDate { get; set; }
        [DisplayName("Due Date")]
        public string DueDate { get; set; }

        [StringLength(1)]
        public string Priority { get; set; }

        public string Status { get; set; }
        public int PageNumber { get; set; }
        public bool FilterActive { get; set; }
    }
}