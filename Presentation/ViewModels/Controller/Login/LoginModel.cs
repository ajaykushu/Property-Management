using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class LoginReq
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Rememberme { set; get; }
    }
}