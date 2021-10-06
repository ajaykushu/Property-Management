using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.User.RequestModels
{
    public class EditUserDTO
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }
        public bool IsEffortVisible { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        public string PrimaryProperty { get; set; }
        public string Suffix { get; set; }

        [Required(ErrorMessage = "Please Choose Role")]
        public string Role { get; set; }

        public List<SelectItem> Roles { get; set; }

        [Required(ErrorMessage = "Please Choose Language")]
        public int Language { get; set; }

        public List<SelectItem> Languages { get; set; }
        public List<SelectItem> Departments { get; set; }

        [Required(ErrorMessage = "Please give Department Id")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Select Time Zone")]
        public string TimeZone { set; get; }

        public List<SelectItem> TimeZones { set; get; }

        [Required(ErrorMessage = "Please Enter  Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please Enter Valid Phone Number")]
        public string PhoneNumber { set; get; }

        

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

        [StringLength(50, ErrorMessage = "Please keep length less than 50")]
        public string OfficeExt { get; set; }

        public string ClockType { get; set; }
        public IFormFile File { get; set; }
    }
}