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
    //[AuthenticationFilter]
    [RoutePrefix("userGenres")]
    public class UserGenreController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public void Insert([FromBody]string username, [FromBody]List<string> genreNames)
        {
            BusinessContext.UserGenreBuisness.Insert(username, genreNames);
        }

        [HttpDelete]
        [Route("{username}/{genreID:Guid}")]
        public void Delete(string username, Guid genreID)
        {
            BusinessContext.UserGenreBuisness.Delete(username, genreID);
        }

        [HttpDelete]
        [Route("{username}")]
        public void DeleteByUsername(string username)
        {
            BusinessContext.UserGenreBuisness.DeleteByUsername(username);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<UserArtist> ReadAll()
        {
            return BusinessContext.UserGenreBuisness.ReadAll();
        }

        [HttpGet]
        [Route("{name}")]
        public UserGenre ReadByUsername(string name)
        {
            return BusinessContext.UserGenreBuisness.ReadByUsername(name);
        }
        #endregion
    }
}
