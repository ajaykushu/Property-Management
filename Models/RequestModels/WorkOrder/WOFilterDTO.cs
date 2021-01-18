using System.ComponentModel.DataAnnotations;

namespace Models.WorkOrder.RequestModels
{
    public class WOFilterDTO
    {
        public string AssignedTo
        {
            get;
            set;
        }
        public string Vendor
        {
            get;
            set;
        }
        public string PropertyName
        {
            get;
            set;
        }
        public string CreationStartDate { get; set; }
        public string CreationEndDate { get; set; }
        public string Status
        {
            get;
            set;
        }
        public string DueDate { get; set; }
        [StringLength(1)]
        public string Priority { get; set; }
        public string WOId
        {
            get;
            set;
        }
        public string TermSearch
        {
            get;
            set;
        }
        public int PageNumber { get; set; }
        public bool IsCurrent { get; set; }
        public bool? IsActive { get; set; }
        public bool SortedByDate { get; set; }//

    }
}