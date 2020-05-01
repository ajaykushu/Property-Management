namespace Presentation.ViewModels
{
    public class WorkOrderAssigned
    {
        public string Id { get; set; }
        public string AssignedToUser { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string Stage { get; set; }
        public SelectItem Property { get; set; }
    }
}