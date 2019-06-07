using EmotionBasedMusicPlayer.Core;
using EmotionBasedMusicPlayer.Filters;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmotionBasedMusicPlayer.Controllers
{
    [AuthenticationFilter]
    [RoutePrefix("userArtists")]
    public class UserArtistController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public void Insert([FromBody]UserArtistPreferences userArtists)
        {
            if (userArtists.Artists.Count > 0)
                BusinessContext.UserArtistBusiness.Insert(userArtists);
        }

        [HttpDelete]
        [Route("{userID:Guid}/{artistID}")]
        public void Delete(Guid userID, string artistID)
        {
            BusinessContext.UserArtistBusiness.Delete(userID, artistID);
        }

        [HttpDelete]
        [Route("{userID:Guid}")]
        public void DeleteByUsername(Guid userID)
        {
            BusinessContext.UserArtistBusiness.DeleteByUserID(userID);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<UserArtist> ReadAll()
        {
            return BusinessContext.UserArtistBusiness.ReadAll();
        }

        [HttpGet]
        [Route("{userID:Guid}")]
        public IEnumerable<Artist> ReadByUsername(Guid userID)
        {
            return BusinessContext.UserArtistBusiness.ReadByUserID(userID);
        }
        #endregion
    }
}
