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
using Newtonsoft.Json.Linq;
using EmotionBasedMusicPlayer.Models;
using EmotionBasedMusicPlayer.Models.Recommendations;
using EmotionBasedMusicPlayer.Filters;
using System.Web;

namespace EmotionBasedMusicPlayer.Controllers
{
    //[AuthenticationFilter]
    [RoutePrefix("recommendations")]
    public class RecommendationController : MainApiController
    {
        #region Methods
        [HttpGet]
        [Route("")]
        public List<Recommendation> GetRecommendations()
        {
            return BusinessContext.GetRecommendations();
        }

        [HttpGet]
        [Route("token")]
        public string GetToken()
        {
            return BusinessContext.RecommendationBusiness.GetToken();
        }

        [HttpGet]
        [Route("genres")]
        public List<string> GetGenreSeeds()
        {
            return BusinessContext.RecommendationBusiness.GetGenreSeeds();
        }

        [HttpGet]
        [Route("artists")]
        public JObject GetArtistSeeds()
        {
            string artistName = HttpContext.Current.Request.Headers["artist"];
            if (artistName == null || String.IsNullOrEmpty(artistName))
                return new JObject();
            return BusinessContext.RecommendationBusiness.GetArtistSeeds(artistName);
        }
        #endregion
    }
}
