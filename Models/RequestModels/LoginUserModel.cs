using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.Password, ErrorMessage = "Please Enter Valid Passoword")]
        public string Password { get; set; }

        public bool Rememberme { get; set; }
    }
}
