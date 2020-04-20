namespace Presentation.ViewModels
{
    public class Properties
    {
        public long Id { set; get; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string StreetAddress1 { set; get; }
        public string StreetAddress2 { set; get; }
        public string ZipCode { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Country { set; get; }
        public bool IsPrimary { set; get; }
        public bool IsActive { get; set; }
    }
}