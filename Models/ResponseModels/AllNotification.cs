namespace Models.ResponseModels
{
    public class AllNotification
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
        public string NavigatorId { get; set; }
        public string CreationTime { get; set; }
    }
}