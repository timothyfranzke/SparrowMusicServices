namespace Sparrow.Services.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
    }

    public class CreateUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ModifyUserModel
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class PasswordResetModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }
}
