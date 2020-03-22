using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.RequestModels
{
    public class RegisterRequest
    {
        [Required]
        public string Title { set; get; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Suffix { get; set; }
        [Required]
        public string Role { get; set; }
        public List<SelectItem> Properties { get; set; }
        public List<string> SelectedProperty { get; set; }
        public List<SelectItem> Roles { get; set; }
        [Required]
        public long Language { get; set; }
        public List<SelectItem> Languages { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public int CountryCode { set; get; }
        public List<SelectItem> CountryCodes { set; get; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }
        public string OfficeExt { get; set; }
        [Required]
        public string TimeZone { set; get; }
        public List<SelectItem> TimeZones { set; get; }
        [Required]
        public string UserName { set; get; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public StringBuilder PhotoStr { get; set; }
    }
}
