using System.ComponentModel.DataAnnotations;

namespace Models.Login.RequestModels
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.Password, ErrorMessage = "Please Enter Valid Passoword")]
        public string Password { get; set; }

        public bool Rememberme { get; set; }
    }
}