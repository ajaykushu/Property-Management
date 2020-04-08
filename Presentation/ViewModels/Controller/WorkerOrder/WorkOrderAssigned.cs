namespace Presentation.ViewModels
{
    public class WorkOrderAssigned
    {
        public long Id { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string AssignedToSection { get; set; }
        public string Stage { get; set; }
    }
}