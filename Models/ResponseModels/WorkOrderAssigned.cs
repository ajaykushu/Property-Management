namespace Models.ResponseModels
{
    public class WorkOrderAssigned
    {
        public string Id { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public bool IsRecurring { get; set; }
        public string DueDate { get; set; }
        public string Stage { get; set; }
        public SelectItem Property { get; set; }
    }
}