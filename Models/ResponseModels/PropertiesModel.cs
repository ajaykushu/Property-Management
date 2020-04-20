namespace Models.ResponseModels
{
    public class PropertiesModel
    {
        public long Id { set; get; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string ZipCode { set; get; }
        public string City { set; get; }
        public string Country { set; get; }
        public bool IsPrimary { set; get; }
        public string StreetAddress2 { get; set; }
        public string StreetAddress1 { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
    }
}