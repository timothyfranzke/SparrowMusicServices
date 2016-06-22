using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using log4net;
using Sparrow.Services.API;
using Sparrow.Services.Models;
using Sparrow.Services.Utils;


namespace Sparrow.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    
    public class AuthController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserController));
        private readonly Security _security;
        private readonly API.User _user;

        public AuthController()
        {
            _security = new Security();
            _user = new User();
        }

        [HttpPost]
        [ActionName("AuthenticateUser")]
        public HttpResponseMessage AuthenticateUser([FromBody]AuthUserModel model)
        {
            try
            { 
                var authModel = _security.AuthenticateUser(model);
                if (authModel.Authenticated)
                    return Request.CreateResponse(HttpStatusCode.OK, authModel);
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        [ActionName("ForgotPassword")]
        public HttpResponseMessage ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            try
            {
                log.Info("ForgotPassword : " + model.Email);
                _user.ResetPassword(model.Email);
                return Request.CreateResponse(HttpStatusCode.OK,"");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ServiceUnavailable);
            }
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public HttpResponseMessage ResetPassword([FromBody] PasswordResetModel model)
        {
            try
            {
                if (_security.Verify(model.Token, model.Email))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, _user.ResetPassword(model.Email, model.Password));
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}
