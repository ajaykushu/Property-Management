using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
    public class EditUserModel
    { 
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }
        public string Suffix { get; set; }
        [Required(ErrorMessage = "Please Choose Role")]
        public string Role { get; set; }
        public List<SelectItem> Roles { get; set; }
        [Required(ErrorMessage = "Please Choose Language")]
        public int Language { get; set; }
        public List<SelectItem> Languages { get; set; }
        [Required(ErrorMessage = "Please Enter Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Choose ContryCode")]
        public int CountryCode { set; get; }
        public List<SelectItem> CountryCodes { set; get; }
        [Required(ErrorMessage = "Please Select Time Zone")]
        public string TimeZone { set; get; }
        public List<SelectItem> TimeZones { set; get; }
        [Required(ErrorMessage = "Please Enter  Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please Enter Valid Phone Number")]
        public string PhoneNumber { set; get; }

        [StringLength(256, MinimumLength = 4, ErrorMessage = "Please Keep length less than 256 charcters")]
        [Required(ErrorMessage = "Please Enter UserName")]
        public string UserName { set; get; }

        [DataType(DataType.Password, ErrorMessage = "Please Enter Valid Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Same")]
        public string ConfirmPassword { get; set; }
        /**/
        public long Id { get; set; }
        public List<SelectItem> Properties { get; set; }
        public List<string> SelectedProperty { get; set; }
        public bool SMSAlert { set; get; }
        public string OfficeExt { get; set; }
        public string ClockType { get; set; }
        //public IFormFile Files { get; set; }


    }
}
