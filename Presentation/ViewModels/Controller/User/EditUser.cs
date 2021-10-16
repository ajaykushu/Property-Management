using Microsoft.AspNetCore.Http;
using Presentation.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class EditUser
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        public string Suffix { get; set; }
        public bool IsEffortVisible { get; set; }

        [Required(ErrorMessage = "Please Choose Role")]
        public string Role { get; set; }

        public List<SelectItem> Roles { get; set; }

        public List<SelectItem> Departments { get; set; }

        [Required(ErrorMessage = "Please Choose Department")]
        [DisplayName("Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Choose Language")]
        public int Language { get; set; }

        public string PrimaryProperty { get; set; }
        public List<SelectItem> Languages { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Select Time Zone")]
        public string TimeZone { set; get; }

        public List<SelectItem> TimeZones { set; get; }

        [DisplayName("Cell Number")]
        [Required(ErrorMessage = "Please Enter  Phone Number")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Enter Valid Phone Number")]
        [StringLength(12)]
        public string PhoneNumber { set; get; }

        [DataType(DataType.Password, ErrorMessage = "Please Enter Valid Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Must have at least 1 number, 1 capital letter and 1 lowercase letter.")]
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

        [DisplayName("Photo")]
        public IFormFile File { get; set; }
    }
}