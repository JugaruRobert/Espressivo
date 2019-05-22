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
            string username = HttpContext.Current.Request.Headers["username"];
            string password = HttpContext.Current.Request.Headers["password"];

            if (username == null || password == null)
                throw new Exception("Unauthorized");

            password = RsaEncryption.Encryption(password);
            BusinessContext context = new BusinessContext();
            User user = context.UserBusiness.ReadUser(username, password);
            if (user == null)
                throw new Exception("Unauthorized");

            return JwtTokenLibrary.GenerateToken(username, user.Email, password);
        } 
        #endregion
    }
}
