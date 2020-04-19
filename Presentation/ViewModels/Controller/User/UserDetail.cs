using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class UserDetail
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public List<Properties> ListProperties { set; get; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string EmailAddress { get; set; }
        public string UserId { get; set; }
        public string OfficeExtension { get; set; }
        public bool SMSAlert { get; set; }
        public bool IsActive { get; set; }
        public string TimeZone { get; set; }
        public string PhotoPath { get; set; }
    }
}