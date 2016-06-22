using Sparrow.Services.Data.Repository;
using Sparrow.Services.Models;
using Sparrow.Services.Utils;

namespace Sparrow.Services.API
{
    public class Security
    {
        private readonly UserRepository UserRepo = new UserRepository();
        private readonly ArtistRepository ArtistRepo = new ArtistRepository();
        public bool Verify(string email, int artistId)
        {
            var count = 0;
            var success = true;
            var artists = ArtistRepo.GetArtists(email);
            foreach (var artist in artists)
            {
                if (artist.ArtistId == artistId)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                success = false;
            }

            return success;
        }

        public bool Verify(string token, string email)
        {
            var user = UserRepo.GetUser(email);
            var success = token.Equals(Auth.GenerateToken((int)user.SALT, user.PASSWORD, user.EMAIL));

            return success;
        }
        public bool Verify(string token, string email, int artistId)
        {
            var success = Verify(token, email);
            if (success)
            {
                success = Verify(email, artistId);
            }

            return success;
        }

        public AuthModel AuthenticateUser(AuthUserModel model)
        {
            var authModel = new AuthModel
            {
                Authenticated = false,
                Token = ""
            };
            var user = UserRepo.GetUser(model.Email);
            var saltedPassword = user.SALT + model.Password;
            var hashedPassword = Auth.GenerateHash(saltedPassword);

            if (hashedPassword.Equals(user.PASSWORD))
            {
                var salt = Auth.GenerateSalt();
                authModel.Authenticated = true;
                saltedPassword = salt + model.Password;
                hashedPassword = Auth.GenerateHash(saltedPassword);
                UserRepo.UpdateSalt(salt, user.EMAIL, hashedPassword);
                authModel.Token = Auth.GenerateToken(salt, hashedPassword, user.EMAIL);
                //authModel.ResetPassword = (bool)user.PASSWORD_RESET;
            }

            return authModel;
        }



    }
}
