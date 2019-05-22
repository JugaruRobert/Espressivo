using EmotionBasedMusicPlayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Runtime.Caching;
using EmotionBasedMusicPlayer.Core;

namespace EmotionBasedMusicPlayer.Controllers
{
    [RoutePrefix("spotify")]
    public class SpotifyController : MainApiController
    {
        #region Methods
        [HttpGet]
        [Route("token")]
        public string GetToken()
        {
            return BusinessContext.SpotifyBusiness.GetToken().Result;
        }

        [HttpGet]
        [Route("recommendations")]
        public void GetRecommendations()
        {
            BusinessContext.SpotifyBusiness.GetRecommendations();
        }

        [HttpGet]
        [Route("genres")]
        public void GetGenreSeeds()
        {
            BusinessContext.SpotifyBusiness.GetGenreSeeds();
        }
        #endregion
    }
}
