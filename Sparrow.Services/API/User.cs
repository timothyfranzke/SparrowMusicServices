using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Security;
using log4net;
using Sparrow.Services.Controllers;
using Sparrow.Services.Data.Repository;
using Sparrow.Services.Models;
using Sparrow.Services.Utils;

namespace Sparrow.Services.API
{
    public static class User
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(User));
        private static readonly UserRepository UserRepo = new UserRepository();
        private static readonly ArtistRepository ArtistRepo = new ArtistRepository();

        public static AuthModel CreateUser(CreateUserModel model)
        {
            //var userInfo = _repository.GetUser(model.Email);
            var pwSalt = Auth.GenerateSalt();
            var saltedPassword = pwSalt + model.Password;
            model.Password = Auth.GenerateHash(saltedPassword);

            var id = UserRepo.CreateUser(model, pwSalt);
            var user = UserRepo.GetUser(id);
            var authUser = new AuthModel();
            if (user != null)
            {
                authUser.Authenticated = true;
                authUser.Token = Auth.GenerateToken((int) user.SALT, user.PASSWORD, user.EMAIL);
            }

            return authUser;
        }

        public static void UpdateUser(ModifyUserModel model)
        {
            UserRepo.UpdateUser(model);
        }

        public static void RemoveUser(int id)
        {
            UserRepo.RemoveUser(id);
        }

        public static void ResetPassword(string email)
        {
            var pwSalt = Auth.GenerateSalt();
            var password = Membership.GeneratePassword(10, 3);
            var saltedPassword = pwSalt + password;
            password = Auth.GenerateHash(saltedPassword);

            log.Info("salt:" + pwSalt + " | password:" + password + " | email:" + email);
            var token = Auth.GenerateToken(pwSalt, password, email);
            log.Info("token:" + token);

            UserRepo.ResetPassword(pwSalt, password, email, true);
            GenerateEmail(email, token);
        }

        public static string ResetPassword(string email, string password)
        {
            var pwSalt = Auth.GenerateSalt();
            var saltedPassword = pwSalt + password;
            password = Auth.GenerateHash(saltedPassword);
            UserRepo.ResetPassword(pwSalt, password, email, false);
            return Auth.GenerateToken(pwSalt, password, email);
        }
        public static FollowedArtistModel GetUsersArtists(string email)
        {
            return UserRepo.GetUsersArtists(email);
        }

        public static FollowedArtistModel FollowArtist(CreateArtistAssociation model)
        {
            ArtistRepo.FollowArtist(model.UserEmail, model.ArtistId);
            return UserRepo.GetUsersArtists(model.UserEmail);
        }

        public static FollowedArtistModel UnFollowArtist(CreateArtistAssociation model)
        {
            ArtistRepo.UnFollowArtist(model.UserEmail, model.ArtistId);
            return UserRepo.GetUsersArtists(model.UserEmail);
        }

        public static IEnumerable<UserModel> SearchUsers(string email)
        {
            return UserRepo.SearchUsers(email);
        }

        public static IEnumerable<EventModel> GetEvents(string email)
        {
            return UserRepo.GetUserArtistEvents(email);
        }

        public static IEnumerable<UserBulliten> GetBullitens(string email)
        {
            return UserRepo.GetUserArtistBullitens(email);
        }

        public static IEnumerable<FilterModel> GetUserFilters(string email)
        {
            return UserRepo.GetFilters(email);
        }

        public static int CreateUserFilter(CreateFilterModel model)
        {
            return UserRepo.CreateFilter(model);
        }

        public static void RemoveUserFilter(int filterId)
        {
            UserRepo.RemoveFilter(filterId);
        }

        private static void GenerateEmail(string email, string token)
        {
            bool EmailIsSent = false;

            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            try
            {
                m.From = new MailAddress("thesparrowteam@sparrowmusic.com", "Sparrow Music");
                m.To.Add(new MailAddress(email));

                m.Subject = "Reset your password";
                m.IsBodyHtml = true;
                m.Body = String.Format("Alright you jabroni (cool word), because it appears you don't internet much, this is called a url, stands for... um... who cares.  Just copy this and past it in your Netscape Navigator or AOL or whatever antiquated piece of junk you're using. http://franzkedesigner.com/sparrow/#/resetPassword/{0}/{1} This will reset your password.",email,token);


                sc.Host = "seagull.arvixe.com";
                sc.Port = 26;
                sc.Credentials = new System.Net.NetworkCredential("sparrow@franzkedesigner.com", "asdf-123");
                //sc.EnableSsl = true;
                sc.Send(m);

                EmailIsSent = true;

            }
            catch (Exception ex)
            {
                
                log.Error("Email Exception Message:" + ex.Message);
                log.Error("Email Exception Stack:" + ex.StackTrace);
                log.Error("Email Exception:" + ex);
                EmailIsSent = false;
            }
        }
    }
}
