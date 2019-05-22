using EmotionBasedMusicPlayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace EmotionBasedMusicPlayer.Filters
{
    public class AuthenticationFilter : Attribute, IAuthenticationFilter
    {
        #region Properties
        public bool AllowMultiple
        {
            get { return false; }
        }
        #endregion

        #region Methods
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != null && authorization.Scheme.ToLower() != "bearer")
                throw new Exception("Invalid scheme. Missing token parameter.");

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                throw new Exception("Missing token parameter.");
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
                throw new Exception("Invalid token.");
            else
                context.Principal = principal;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            IPrincipal principal;

            if (ValidateToken(token, out principal))
            {
                return Task.FromResult(principal);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        private bool ValidateToken(string token, out IPrincipal simplePrinciple)
        {
            string username = null;
            string email = null;

            simplePrinciple = JwtTokenLibrary.GetPrincipal(token);
            if (simplePrinciple == null)
                return false;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            var emailClaim = identity.FindFirst(ClaimTypes.Email);
            email = emailClaim?.Value;

            if (string.IsNullOrEmpty(email))
                return false;

            return true;
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken) { }
        #endregion
    }
}