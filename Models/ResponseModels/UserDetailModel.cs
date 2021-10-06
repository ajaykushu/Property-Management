using System.Collections.Generic;

namespace Models.ResponseModels
{
    public class UserDetailModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public IList<string> Roles { get; set; }
        public List<PropertiesModel> ListProperties { set; get; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string EmailAddress { get; set; }
       
        public bool IsEffortVisible { get; set; }
        public string OfficeExtension { get; set; }
        public bool SMSAlert { get; set; }
        public bool IsActive { get; set; }
        public string TimeZone { get; set; }
        public string PhotoPath { get; set; }
        public List<SelectItem> TimeZones { get; set; }
    }
}