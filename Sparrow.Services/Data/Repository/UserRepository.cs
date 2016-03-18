using System;
using System.Collections.Generic;
using System.Linq;
using Sparrow.Services.Models;

namespace Sparrow.Services.Data.Repository
{
    public class UserRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int CreateUser(CreateUserModel model, int salt)
        {
            try
            {
                int id = -1;
                using (var context = new sparrow_dbEntities())
                {
                    var user = new SPRW_USER
                    {
                        FIRST_NAME = model.FirstName,
                        LAST_NAME = model.LastName,
                        EMAIL = model.Email,
                        SALT = salt,
                        CC = "",
                        PASSWORD = model.Password,
                        LAST_MAINT_TIME = DateTime.Now,
                        LAST_MAINT_USER_ID = model.Email
                    };

                    context.SPRW_USER.Add(user);
                    context.SaveChanges();

                    id = user.USER_ID;
                }

                return id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateUser(ModifyUserModel model)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var user = context.SPRW_USER.FirstOrDefault(i => i.USER_ID == model.UserId);
                    if (user != null)
                    {
                        user.EMAIL = model.Email;
                        user.FIRST_NAME = model.FirstName;
                        user.LAST_NAME = model.LastName;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveUser(int id)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var user = context.SPRW_USER.FirstOrDefault(i => i.USER_ID == id);
                    if (user != null)
                    {
                        user.ACT_IND = false;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void ResetPassword(int salt, string password, string userEmail, bool newPassword)
        {
            var id = GetUserId(userEmail);
            using (var context = new sparrow_dbEntities())
            {
                var selectedUser = context.SPRW_USER.FirstOrDefault(i => i.USER_ID == id);
                if (selectedUser != null)
                {
                    selectedUser.SALT = salt;
                    selectedUser.PASSWORD = password;
                    selectedUser.PASSWORD_RESET = newPassword;
                }
                context.SaveChanges();
            }
        }

        public void UpdateSalt(int salt, string userEmail, string passwordHash)
        {
            var id = GetUserId(userEmail);
            using (var context = new sparrow_dbEntities())
            {
                var selectedUser = context.SPRW_USER.FirstOrDefault(i => i.USER_ID == id);
                if (selectedUser != null)
                {
                    selectedUser.SALT = salt;
                    selectedUser.PASSWORD = passwordHash;
                }
                context.SaveChanges();
            }
        }
        public SPRW_USER GetUser(int id)
        {
            var user = new SPRW_USER();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var selectedUser = context.SPRW_USER.FirstOrDefault(i => i.USER_ID.Equals(id));
                    if (selectedUser != null)
                    {
                        user.ACT_IND = selectedUser.ACT_IND;
                        user.CC = selectedUser.CC;
                        user.EMAIL = selectedUser.EMAIL;
                        user.FIRST_NAME = selectedUser.FIRST_NAME;
                        user.LAST_MAINT_TIME = selectedUser.LAST_MAINT_TIME;
                        user.LAST_MAINT_USER_ID = selectedUser.LAST_MAINT_USER_ID;
                        user.LAST_NAME = selectedUser.LAST_NAME;
                        user.PASSWORD = selectedUser.PASSWORD;
                        user.SALT = selectedUser.SALT;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return user;
        }
        public SPRW_USER GetUser(string userEmail)
        {
            var user = new SPRW_USER();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var selectedUser = context.SPRW_USER.FirstOrDefault(i => i.EMAIL.ToLower().Equals(userEmail.ToLower()));
                    if (selectedUser != null)
                    {
                        user.ACT_IND = selectedUser.ACT_IND;
                        user.CC = selectedUser.CC;
                        user.EMAIL = selectedUser.EMAIL;
                        user.FIRST_NAME = selectedUser.FIRST_NAME;
                        user.LAST_MAINT_TIME = selectedUser.LAST_MAINT_TIME;
                        user.LAST_MAINT_USER_ID = selectedUser.LAST_MAINT_USER_ID;
                        user.LAST_NAME = selectedUser.LAST_NAME;
                        user.PASSWORD = selectedUser.PASSWORD;
                        user.SALT = selectedUser.SALT;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return user;
        }

        public FollowedArtistModel GetUsersArtists(string email)
        {
            using (var context = new sparrow_dbEntities())
            {
                var user = context.SPRW_USER.FirstOrDefault(i => i.EMAIL == email);
                var artists = user.SPRW_ARTIST.ToList();
                var userArtists = new FollowedArtistModel
                {
                    ArtistIds = new List<int>(),
                    LikedTrackIds = context.SPRW_TRACK_POPULAR_LIKE.Where(i=>i.USER_ID == user.USER_ID).Select(i=>i.TRACK_ID).ToList(),
                    Events = new List<EventModel>(),
                    Bulliten = new List<BullitenModel>()
                };

                foreach (var artist in artists)
                {
                    userArtists.ArtistIds.Add(artist.ARTIST_ID);
                    var bullitens = artist.ARTIST_BLOG.ToList();
                    foreach (var bulliten in bullitens)
                    {
                        var userBulliten = new BullitenModel
                        {
                            ArtistId = artist.ARTIST_ID,
                            AristName = artist.NAME,
                            Bulliten = bulliten.BLOG
                        };
                        userArtists.Bulliten.Add(userBulliten);
                    }
                    var evts = artist.SPRW_ARTIST_EVENT.OrderByDescending(i=>i.EVENT_DATE).ToList();
                    foreach (var evt in evts)
                    {
                        var userEvent = new EventModel
                        {
                            Address = evt.ADDRESS,
                            ArtistId = artist.ARTIST_ID,
                            ArtistName = artist.NAME,
                            Description = evt.DESCRP,
                            EventDate = evt.EVENT_DATE,
                            City = evt.CITY,
                            State = evt.STATE,
                            Url = evt.URL,
                            Name = evt.NAME
                        };
                        userArtists.Events.Add(userEvent);
                    }

                }
                return userArtists;
            }
        }

        public IEnumerable<UserBulliten> GetUserArtistBullitens(string email)
        {
            var validDate = DateTime.Now.AddDays(-1);
            var userId = GetUserId(email);
            var userArtistBullitens = new List<UserBulliten>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var user = context.SPRW_USER.FirstOrDefault(i => i.USER_ID == userId);
                    if (user != null)
                    {
                        var bullitens =
                            user.SPRW_ARTIST.Select(
                                i => i.ARTIST_BLOG.FirstOrDefault(j => j.LAST_MAINT_TIME >= validDate));

                        foreach (var bulliten in bullitens)
                        {
                            var model = new UserBulliten
                            {
                                AristName = bulliten.SPRW_ARTIST.NAME,
                                Bulliten = bulliten.BLOG
                            };
                            userArtistBullitens.Add(model);
                        }

                    }
                }
                return userArtistBullitens;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public IEnumerable<EventModel> GetUserArtistEvents(string email)
        {
            var validDate = DateTime.Now.AddDays(-1);
            var userId = GetUserId(email);
            var userArtistEvents = new List<EventModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var user = context.SPRW_USER.FirstOrDefault(i => i.USER_ID == userId);
                    if (user != null)
                    {
                        var artists = user.SPRW_ARTIST;
                        foreach (var artist in artists)
                        {
                            var evts = artist.SPRW_ARTIST_EVENT.Where(i => i.EVENT_DATE > validDate && i.ACT_IND == true);
                            foreach (var evt in evts)
                            {
                                var model = new EventModel
                                {
                                    Address = evt.ADDRESS,
                                    ArtistId = evt.ARTIST_ID,
                                    ArtistName = evt.SPRW_ARTIST.NAME,
                                    City = evt.CITY,
                                    Description = evt.DESCRP,
                                    EventDate = evt.EVENT_DATE,
                                    Name = evt.NAME,
                                    State = evt.STATE,
                                    Url = evt.URL
                                };
                                userArtistEvents.Add(model);
                            }
                        }
                    }
                }
                return userArtistEvents;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public IEnumerable<UserModel> SearchUsers(string email)
        {
            var userList = new List<UserModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var users = context.SPRW_USER.Where(i => i.EMAIL.Contains(email));
                    foreach (var user in users)
                    {
                        var model = new UserModel()
                        {
                            UserEmail = user.EMAIL,
                            UserId = user.USER_ID
                        };
                        userList.Add(model);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return userList;
        }

        public IEnumerable<FilterModel> GetFilters(string userEmail)
        {
            var userId = GetUserId(userEmail);
            var filterList = new List<FilterModel>();
            using (var context = new sparrow_dbEntities())
            {
                var filters = context.SPRW_USER_FILTER.Where(i => i.USER_ID == userId);
                foreach (var filter in filters)
                {
                    var model = new FilterModel
                    {
                        Filter = filter.FILTER,
                        FilterId = filter.FILTER_ID,
                        Name = filter.NAME
                    };
                    filterList.Add(model);
                }
            }

            return filterList;
        }  

        public int CreateFilter(CreateFilterModel model)
        {
            var userId = GetUserId(model.UserEmail);
            var filterId = -1;
            using (var context = new sparrow_dbEntities())
            {
                var user = context.SPRW_USER.FirstOrDefault(i => i.USER_ID == userId);
                if (user != null)
                {
                    var filter = new SPRW_USER_FILTER
                    {
                        ACT_IND = true,
                        FILTER = model.Filter,
                        LAST_MAINT_TIME = DateTime.Now,
                        LAST_MAINT_USER = model.UserEmail,
                        NAME = model.Name,
                        USER_ID = userId
                    };
                    context.SPRW_USER_FILTER.Add(filter);
                    context.SaveChanges();

                    filterId = filter.FILTER_ID;

                }
            }
            return filterId;
        }

        public void RemoveFilter(int filterId)
        {
            using (var context = new sparrow_dbEntities())
            {
                var filter = context.SPRW_USER_FILTER.FirstOrDefault(i => i.FILTER_ID == filterId);
                if (filter != null)
                {
                    filter.ACT_IND = false;
                }
                context.SaveChanges();
            }
        }

        #region Private Methods
        private int GetUserId(string email)
        {
            var id = -1;
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var firstOrDefault = context.SPRW_USER.FirstOrDefault(i => i.EMAIL.ToLower().Equals(email.ToLower()));
                    if (firstOrDefault != null)
                        id = firstOrDefault.USER_ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return id;
        }
        #endregion
    }
}
