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
        [Route("{username}/{genreID:Guid}")]
        public void Delete(string username, Guid genreID)
        {
            BusinessContext.UserGenreBusiness.Delete(username, genreID);
        }

        [HttpDelete]
        [Route("{username}")]
        public void DeleteByUsername(string username)
        {
            BusinessContext.UserGenreBusiness.DeleteByUsername(username);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<UserGenre> ReadAll()
        {
            return BusinessContext.UserGenreBusiness.ReadAll();
        }

        [HttpGet]
        [Route("{name}")]
        public UserGenre ReadByUsername(string name)
        {
            return BusinessContext.UserGenreBusiness.ReadByUsername(name);
        }
        #endregion
    }
}
