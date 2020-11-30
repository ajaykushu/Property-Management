namespace Models.ResponseModels
{
    public class WorkOrderAssigned
    {
        public string Id { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; }
        public SelectItem Property { get; set; }
        public bool IsActive { get; set; }
    }
}