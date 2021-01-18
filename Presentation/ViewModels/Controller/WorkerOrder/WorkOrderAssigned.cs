using Presentation.Utility;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class WorkOrderAssigned
    {   [DisplayName("WO#")]
        [Key]
        public string Id { get; set; }
        [DisplayName("Assigned To")]
        public string AssignedTo { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Parent Id")]
        public string ParentId { get; set; }
        [DisplayName("Due Date")]
        public string DueDate { get; set; }
        [DisplayName("Property")]
        [SkipPropertyAttribute]
        public SelectItem Property { get; set; }
        [SkipPropertyAttribute]
        public bool IsActive { get; set; }

    }
}