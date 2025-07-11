using System.ComponentModel.DataAnnotations;

namespace Asm2.DataTransferModels.Identity
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid format!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password can not be empty.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Account need to be a patient or a doctor!")]
        public string Role { get; set; }
    }
}
