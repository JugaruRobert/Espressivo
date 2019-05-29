﻿using EmotionBasedMusicPlayer.Core;
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
        [Route("{username}/{artistID}")]
        public void Delete(string username, string artistID)
        {
            BusinessContext.UserArtistBusiness.Delete(username, artistID);
        }

        [HttpDelete]
        [Route("{username}")]
        public void DeleteByUsername(string username)
        {
            BusinessContext.UserArtistBusiness.DeleteByUsername(username);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<UserArtist> ReadAll()
        {
            return BusinessContext.UserArtistBusiness.ReadAll();
        }

        [HttpGet]
        [Route("{name}")]
        public UserGenre ReadByUsername(string name)
        {
            return BusinessContext.UserArtistBusiness.ReadByUsername(name);
        }
        #endregion
    }
}
