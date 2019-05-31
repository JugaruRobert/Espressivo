using EmotionBasedMusicPlayer.Core;
using EmotionBasedMusicPlayer.Filters;
using EmotionBasedMusicPlayer.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmotionBasedMusicPlayer.Controllers
{
    //[AuthenticationFilter]
    [RoutePrefix("userGenres")]
    public class UserGenreController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public void Insert([FromBody]UserGenrePreferences userGenres)
        {
            if (userGenres.Genres.Count > 0)
                BusinessContext.UserGenreBusiness.Insert(userGenres);
        }

        [HttpDelete]
        [Route("{userID:Guid}/{genreID:Guid}")]
        public void Delete(Guid userID, Guid genreID)
        {
            BusinessContext.UserGenreBusiness.Delete(userID, genreID);
        }

        [HttpDelete]
        [Route("{userID:Guid}")]
        public void DeleteByUsername(Guid userID)
        {
            BusinessContext.UserGenreBusiness.DeleteByUserID(userID);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<UserGenre> ReadAll()
        {
            return BusinessContext.UserGenreBusiness.ReadAll();
        }

        [HttpGet]
        [Route("{userID:Guid}")]
        public UserGenre ReadByUsername(Guid userID)
        {
            return BusinessContext.UserGenreBusiness.ReadByUserID(userID);
        }
        #endregion
    }
}
