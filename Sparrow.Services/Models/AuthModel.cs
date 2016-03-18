namespace Sparrow.Services.Models
{
    public class AuthModel
    {
        public string Token { get; set; }
        public bool Authenticated { get; set; }
        public bool ResetPassword { get; set; }
    }

    public class AuthUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
