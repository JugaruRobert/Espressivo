using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace EmotionBasedMusicPlayer.Core
{
    public class MainApiController : ApiController, IDisposable
    {
        #region Members
        private BusinessContext _businessContext;
        #endregion

        #region Properties
        public BusinessContext BusinessContext
        {
            get
            {
                if (_businessContext == null)
                {
                    _businessContext = new BusinessContext();
                    //_businessContext = GetAuthenticatedBusinessContext();
                    //if (_businessContext == null)
                    //throw new Exception("Unauthorized access!");
                }
                return _businessContext;
            }
        }
        #endregion

        #region Methods
        private BusinessContext GetAuthenticatedBusinessContext()
        {
            ClaimsIdentity currentIdentity = (ClaimsIdentity)RequestContext.Principal.Identity;
            if (currentIdentity == null)
                return null;

            var usernameClaim = currentIdentity.FindFirst(ClaimTypes.Name);
            string username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return null;

            var passwordClaim = currentIdentity.FindFirst(ClaimValueTypes.Rsa);
            string password = passwordClaim?.Value;

            if (string.IsNullOrEmpty(password))
                return null;

            string decryptedPassword = RsaEncryption.Decryption(password);

            var emailClaim = currentIdentity.FindFirst(ClaimTypes.Email);
            string email = emailClaim?.Value;

            if (string.IsNullOrEmpty(email))
                return null;

            User authenticatedUser = new User()
            {
                Username = username,
                Password = decryptedPassword,
                Email = email
            };

            return new BusinessContext(authenticatedUser);
        }
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_businessContext != null)
                {
                    _businessContext.Dispose();
                    _businessContext = null;
                }
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
