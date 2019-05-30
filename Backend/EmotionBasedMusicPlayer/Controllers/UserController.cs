using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Core;
using EmotionBasedMusicPlayer.Filters;
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
    //[AuthenticationFilter]
    [RoutePrefix("users")]
    public class UserController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public void Insert([FromBody]User user)
        {
            user.Password = AesEncryption.Encrypt(user.Password);
            BusinessContext.UserBusiness.Insert(user);
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
            User user = context.UserBusiness.ReadByUsernameAndEmail(username,email);
            if (user != null)
            {
                string errorMessage = String.Empty;
                if (username == user.Username)
                    errorMessage = "Error.ExistingUsername";
                else if(email == user.Email)
                    errorMessage = "Error.ExistingEmail";
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMessage);
                throw new HttpResponseException(response);
            }

            context.UserBusiness.Insert(new Models.User()
            {
                Username = username,
                Email = email,
                Password = password
            });

            return JwtTokenLibrary.GenerateToken(username, email, password);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<User> ReadAll()
        {
            return BusinessContext.UserBusiness.ReadAll();
        }

        [HttpGet]
        [Route("{username}")]
        public User ReadByID(string username)
        {
            return BusinessContext.UserBusiness.ReadByID(username);
        }

        [HttpPost]
        [Route("update")]
        public void Update([FromBody]User user)
        {
            BusinessContext.UserBusiness.Update(user);
        }

        [HttpDelete]
        [Route("{username}")]
        public void Delete(string username)
        {
            BusinessContext.UserBusiness.Delete(username);
        }
        #endregion
    }
}
