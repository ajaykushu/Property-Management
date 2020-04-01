using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class PasswordChange
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        public string ReEnterPassword { get; set; }
    }
}