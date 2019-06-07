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
    [AuthenticationFilter]
    [RoutePrefix("users")]
    public class UserController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public void Insert([FromBody]User user)
        {
            user.Password = AesEncryption.Encrypt(user.Password);
            user.UserID = Guid.NewGuid();
            BusinessContext.UserBusiness.Insert(user);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<User> ReadAll()
        {
            return BusinessContext.UserBusiness.ReadAll();
        }

        [HttpGet]
        [Route("preferences")]
        public UserPreferences GetPreferences()
        {
            string userID = HttpContext.Current.Request.Headers["userID"];
            if (userID == null)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error.MissingUserID");
                throw new HttpResponseException(response);
            }
            return BusinessContext.UserBusiness.GetPreferences(new Guid(userID));
        }

        [HttpGet]
        [Route("{userID:Guid}")]
        public User ReadByID(Guid userID)
        {
            return BusinessContext.UserBusiness.ReadByID(userID);
        }


        [HttpGet]
        [Route("{username}")]
        public User ReadByUsername(string username)
        {
            return BusinessContext.UserBusiness.ReadByUsername(username);
        }

        [HttpPost]
        [Route("update")]
        public void Update([FromBody]User user)
        {
            User existingUser = BusinessContext.UserBusiness.ReadByUsernameOrEmail(user.UserID, user.Username, user.Email);
            if (existingUser != null)
            {
                string errorMessage = String.Empty;
                if (user.Username == existingUser.Username)
                    errorMessage = "Error.ExistingUsername";
                else if (user.Email == existingUser.Email)
                    errorMessage = "Error.ExistingEmail";
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, errorMessage);
                throw new HttpResponseException(response);
            }
            BusinessContext.UserBusiness.Update(user);
        }

        [HttpDelete]
        [Route("{userID:Guid}")]
        public void Delete(Guid userID)
        {
            BusinessContext.UserBusiness.DeleteByID(userID);
        }

        [HttpDelete]
        [Route("{username}")]
        public void Delete(string username)
        {
            BusinessContext.UserBusiness.DeleteByUsername(username);
        }
        #endregion
    }
}
