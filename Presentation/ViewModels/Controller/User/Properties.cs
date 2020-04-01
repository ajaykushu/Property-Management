namespace Presentation.ViewModels
{
    public class Properties
    {
        public long Id { set; get; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string HouseNumber { set; get; }
        public string Locality { set; get; }
        public string Street { set; get; }
        public string StreetLine2 { set; get; }
        public string PinCode { set; get; }
        public string City { set; get; }
        public string Country { set; get; }
        public bool IsPrimary { set; get; }
    }
}