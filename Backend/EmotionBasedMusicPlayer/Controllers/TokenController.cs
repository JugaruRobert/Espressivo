using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EmotionBasedMusicPlayer.Controllers
{
    [RoutePrefix("token")]
    public class TokenController : MainApiController
    {
        #region Methods
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public string GetToken()
        {
            BusinessContext refreshTokenContext = GetAuthenticatedBusinessContext();
            if (refreshTokenContext != null)
                return JwtTokenLibrary.GenerateToken(refreshTokenContext.CurrentUser.UserID, refreshTokenContext.CurrentUser.Username, refreshTokenContext.CurrentUser.Email);

            string username = HttpContext.Current.Request.Headers["username"];
            string password = HttpContext.Current.Request.Headers["password"];

            if (username == null || password == null)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error.MissingCredentials");
                throw new HttpResponseException(response);
            }

            password = AesEncryption.Encrypt(password);
            BusinessContext context = new BusinessContext();
            User user = context.UserBusiness.ReadUser(username, password);
            if (user == null)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error.InvalidCredentials");
                throw new HttpResponseException(response);
            }

            return JwtTokenLibrary.GenerateToken(user.UserID, username, user.Email);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public string Register()
        {
            string username = HttpContext.Current.Request.Headers["username"];
            string email = HttpContext.Current.Request.Headers["email"];
            string password = HttpContext.Current.Request.Headers["password"];

            if (username == null || email == null || password == null)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error.MissingCredentials");
                throw new HttpResponseException(response);
            }

            password = AesEncryption.Encrypt(password);
            BusinessContext context = new BusinessContext();
            Guid userID = Guid.NewGuid();
            User user = context.UserBusiness.ReadByUsernameOrEmail(userID, username, email);
            if (user != null)
            {
                string errorMessage = String.Empty;
                if (username == user.Username)
                    errorMessage = "Error.ExistingUsername";
                else if (email == user.Email)
                    errorMessage = "Error.ExistingEmail";
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMessage);
                throw new HttpResponseException(response);
            }

            context.UserBusiness.Insert(new Models.User()
            {
                UserID = userID,
                Username = username,
                Email = email,
                Password = password
            });

            return JwtTokenLibrary.GenerateToken(userID, username, email);
        }
        #endregion
    }
}
