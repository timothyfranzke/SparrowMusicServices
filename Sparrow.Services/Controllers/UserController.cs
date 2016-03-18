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

        [HttpPost]
        [ActionName("User")]
        public HttpResponseMessage CreateUser([FromBody]CreateUserModel model)
        {
            try
            {
                var authModel = API.User.CreateUser(model);
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
                if (Security.Verify(model.Token, model.Email))
                {
                    API.User.UpdateUser(model);
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
                if (Security.Verify(model.Token, model.Email))
                {
                    API.User.RemoveUser(model.UserId);
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
                return Request.CreateResponse(HttpStatusCode.OK, API.User.GetUsersArtists(email));
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
                return Request.CreateResponse(HttpStatusCode.OK, API.User.GetEvents(email));
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
                return Request.CreateResponse(HttpStatusCode.OK, API.User.GetBullitens(email));
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
                if (Security.Verify(model.Token, model.UserEmail))
                {
                    var artists = API.User.FollowArtist(model);
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
                if (Security.Verify(model.Token, model.UserEmail))
                {
                    var artists = API.User.UnFollowArtist(model);
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
                if (Security.Verify(token, userEmail))
                {
                    var filters = API.User.GetUserFilters(userEmail);
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
                if (Security.Verify(model.Token, model.UserEmail))
                {
                    var id = API.User.CreateUserFilter(model);
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
                if (Security.Verify(token, userEmail))
                {
                    API.User.RemoveUserFilter(filterId);
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
