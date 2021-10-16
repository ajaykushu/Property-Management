using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Presentation.Utility;

namespace Presentation.ViewModels
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }
        public bool IsEffortVisible { get; set; }

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

        public string OfficeExt { get; set; }
      

        [DisplayName("Cell Number")]
        [Remote("CheckPhoneNumber", "User")]
        [Required(ErrorMessage = "Please Enter  Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enter Valid Phone Number")]
        [StringLength(12)]
        public string PhoneNumber { set; get; }
       
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password, ErrorMessage = "Please Enter Valid Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Must have at least 1 number, 1 capital letter and 1 lowercase letter.")]
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