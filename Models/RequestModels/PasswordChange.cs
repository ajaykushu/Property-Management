﻿using System.ComponentModel.DataAnnotations;


namespace Models.RequestModels
{
    public class PasswordChange
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
