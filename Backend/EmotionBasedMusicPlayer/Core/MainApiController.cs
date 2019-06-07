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
                    //_businessContext = new BusinessContext();
                    _businessContext = GetAuthenticatedBusinessContext();
                    if (_businessContext == null)
                    {
                        HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error.Unauthorized");
                        throw new HttpResponseException(response);
                    }
                }
                return _businessContext;
            }
        }
        #endregion

        #region Methods
        protected BusinessContext GetAuthenticatedBusinessContext()
        {
            ClaimsIdentity currentIdentity = (ClaimsIdentity)RequestContext.Principal.Identity;
            if (currentIdentity == null)
                return null;

            var userIDClaim = currentIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userIDString = userIDClaim?.Value;

            if (string.IsNullOrEmpty(userIDString))
                return null;

            Guid userID = new Guid(userIDString);

            var usernameClaim = currentIdentity.FindFirst(ClaimTypes.Name);
            string username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return null;

            var emailClaim = currentIdentity.FindFirst(ClaimTypes.Email);
            string email = emailClaim?.Value;

            if (string.IsNullOrEmpty(email))
                return null;

            User authenticatedUser = new User()
            {
                UserID = userID,
                Username = username,
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
