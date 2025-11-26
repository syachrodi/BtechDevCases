using System.ComponentModel.DataAnnotations;

namespace TakeHomeAssignment.General.AccountAPI.Model.General
{
    public class RegisterUserParam
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserParam
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
