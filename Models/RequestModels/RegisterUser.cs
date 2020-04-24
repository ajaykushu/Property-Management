﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
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

        public List<SelectItem> Properties { get; set; }
        public List<string> SelectedProperty { get; set; }
        public List<SelectItem> Roles { get; set; }

        [Required(ErrorMessage = "Please Choose Language")]
        public int Language { get; set; }
        public string PrimaryProperty { get; set; }

        public List<SelectItem> Languages { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Select Time Zone")]
        public string TimeZone { set; get; }

        [StringLength(50, MinimumLength = 0, ErrorMessage = "Please keep length less than 50")]
        public string OfficeExt { get; set; }

        public List<SelectItem> TimeZones { set; get; }
        public List<SelectItem> Departments { get; set; }

        [Required(ErrorMessage = "Please give Department Id")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Enter  Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please Enter Valid Phone Number")]
        public string PhoneNumber { set; get; }

        [StringLength(256, MinimumLength = 4, ErrorMessage = "Please Keep length less than 256 charcters")]
        [Required(ErrorMessage = "Please Enter UserName")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password, ErrorMessage = "Please Enter Valid Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Same")]
        public string ConfirmPassword { get; set; }

        public IFormFile File { get; set; }
    }
}