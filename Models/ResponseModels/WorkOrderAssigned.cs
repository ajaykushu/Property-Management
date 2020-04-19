namespace Models.ResponseModels
{
    public class WorkOrderAssigned
    {
        public long Id { get; set; }
        public string AssignedToUser { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string Stage { get; set; }
        public SelectItem Property { get; set; }
    }
}