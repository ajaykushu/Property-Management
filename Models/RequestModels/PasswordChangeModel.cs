using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
    public class PasswordChangeModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Change Token Not Found Contact Admin")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}