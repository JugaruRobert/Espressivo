using EmotionBasedMusicPlayer.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            user.Password = RsaEncryption.Encryption(user.Password);
            BusinessContext.UserBusiness.Insert(user);
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

        [HttpPut]
        [Route("")]
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
