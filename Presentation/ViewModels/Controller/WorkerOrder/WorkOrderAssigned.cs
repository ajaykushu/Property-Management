namespace Presentation.ViewModels
{
    public class WorkOrderAssigned
    {
        public string Id { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public bool Recurring { get; set; }
        public string DueDate { get; set; }
        public SelectItem Property { get; set; }
    }
}