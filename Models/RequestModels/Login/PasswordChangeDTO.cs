using System.ComponentModel.DataAnnotations;

namespace Models.Login.RequestModels
{
    public class PasswordChangeDTO
    {
        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Change Token Not Found Contact Admin")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}