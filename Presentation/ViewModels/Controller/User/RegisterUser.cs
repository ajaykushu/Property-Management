using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        public string Suffix { get; set; }

        [Required(ErrorMessage = "Please Choose Role")]
        public string Role { get; set; }

        public List<SelectItem> Roles { get; set; }
        public List<SelectItem> Departments { get; set; }

        [Required(ErrorMessage = "Please Choose Department")]
        [DisplayName("Department")]
        public int DepartmentId { get; set; }

        public List<SelectItem> Properties { get; set; }
        public List<string> SelectedProperty { get; set; }

        [Required(ErrorMessage = "Please Choose Language")]
        public int Language { get; set; }

        public List<SelectItem> Languages { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Address")]
        [Remote("CheckEmail", "User")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Select Time Zone")]
        public string TimeZone { set; get; }

        public string OfficeExt { get; set; }
        public List<SelectItem> TimeZones { set; get; }

        [DisplayName("Cell Number")]
        [Remote("CheckPhoneNumber", "User")]
        [Required(ErrorMessage = "Please Enter  Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please Enter Valid Phone Number")]
        public string PhoneNumber { set; get; }

        [Remote("CheckUserName", "User")]
        [StringLength(256, MinimumLength = 4, ErrorMessage = "Please Keep length between 4 to 256 charcters")]
        [Required(ErrorMessage = "Please Enter UserName")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password, ErrorMessage = "Please Enter Valid Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Same")]
        public string ConfirmPassword { get; set; }
        [DisplayName("Primary Property")]
        public string PrimaryProperty { get; set; }
        [DisplayName("Photo")]
        public IFormFile File { get; set; }
    }
}