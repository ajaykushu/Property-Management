using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class WOFilterModel
    {
        [DisplayName("Assigned To")]
        public string AssignedTo { get; set; }
        [DisplayName("Property Name")]
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
        public string Vendor { get; set; }
        [DisplayName("Term Search")]
        public string TermSearch { get; set; }
        public int PageNumber { get; set; }
        [DisplayName("Work Order Number")]
        public string WOId { get; set; }
        public bool FilterActive { get; set; }
        public bool IsCurrent { get;set; }
        public bool? IsActive { get; set; }
        public bool isGeneralSearch { set; get; }
        public bool SortedByDate { get; set; }//
    }
}