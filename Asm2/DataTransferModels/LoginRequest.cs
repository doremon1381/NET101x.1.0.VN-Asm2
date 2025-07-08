using System.ComponentModel.DataAnnotations;

namespace Asm2.DataTransferModels
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username can not be empty")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password can not be empty")]
        public string Password { get; set; }
    }
}
