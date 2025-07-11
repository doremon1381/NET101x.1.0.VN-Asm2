using System;

namespace Asm2.DataTransferModels.Identity
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires_in { get; set; }
    }
}
