using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Sparrow.Services.API;
using Sparrow.Services.Models;
using log4net;

namespace Sparrow.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserController));
        private readonly User _user;
        private readonly Security _security;

        public UserController()
        {
            _user = new User();
            _security = new Security();
        }
        [HttpPost]
        [ActionName("User")]
        public HttpResponseMessage CreateUser([FromBody]CreateUserModel model)
        {
            try
            {
                var authModel = _user.CreateUser(model);
                var response = Request.CreateResponse(HttpStatusCode.Created, authModel);
                return response;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ActionName("User")]
        public HttpResponseMessage UpdateUser([FromBody]ModifyUserModel model)
        {
            try
            {
                if (_security.Verify(model.Token, model.Email))
                {
                    _user.UpdateUser(model);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ActionName("User")]
        public HttpResponseMessage RemoveUser([FromBody]ModifyUserModel model)
        {
            try
            {
                if (_security.Verify(model.Token, model.Email))
                {
                    _user.RemoveUser(model.UserId);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Artists")]
        public HttpResponseMessage GetFollowedArtists(string email)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _user.GetUsersArtists(email));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpGet]
        [ActionName("Events")]
        public HttpResponseMessage GetFollowedArtistsEvents(string email)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _user.GetEvents(email));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Bullitens")]
        public HttpResponseMessage GetFollowedArtistsBullitens(string email)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _user.GetBullitens(email));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [ActionName("Follow")]
        public HttpResponseMessage Follow([FromBody]CreateArtistAssociation model)
        {
            try
            {
                if (_security.Verify(model.Token, model.UserEmail))
                {
                    var artists = _user.FollowArtist(model);
                    return Request.CreateResponse(HttpStatusCode.OK, artists);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [ActionName("Unfollow")]
        public HttpResponseMessage UnFollow([FromBody]CreateArtistAssociation model)
        {
            try
            {
                if (_security.Verify(model.Token, model.UserEmail))
                {
                    var artists = _user.UnFollowArtist(model);
                    return Request.CreateResponse(HttpStatusCode.OK, artists);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [ActionName("Filter")]
        [HttpGet]
        public HttpResponseMessage GetUserFilters(string userEmail, string token)
        {
            try
            {
                if (_security.Verify(token, userEmail))
                {
                    var filters = _user.GetUserFilters(userEmail);
                    return Request.CreateResponse(HttpStatusCode.OK, filters);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [ActionName("Filter")]
        [HttpPost]
        public HttpResponseMessage CreateUserFilters([FromBody]CreateFilterModel model)
        {
            try
            {
                if (_security.Verify(model.Token, model.UserEmail))
                {
                    var id = _user.CreateUserFilter(model);
                    return Request.CreateResponse(HttpStatusCode.OK, id);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [ActionName("Filter")]
        [HttpDelete]
        public HttpResponseMessage RemoveUserFilters(string userEmail, string token, int filterId)
        {
            try
            {
                if (_security.Verify(token, userEmail))
                {
                    _user.RemoveUserFilter(filterId);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
