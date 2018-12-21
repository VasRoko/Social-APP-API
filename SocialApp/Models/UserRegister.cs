using System.ComponentModel.DataAnnotations;

namespace SocialApp.Models
{
    public class UserRegister
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Your password must be between 8 and 255 characters long")]
        public string Password { get; set; }
    }
}
