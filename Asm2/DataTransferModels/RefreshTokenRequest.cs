using System.ComponentModel.DataAnnotations;

namespace Asm2.DataTransferModels
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token can not be empty!")]
        public string RefreshToken { get; set; }
        [Required(ErrorMessage = "AccessToken can not be empty!")]
        public string AccessToken { get; set; }
    }
}
