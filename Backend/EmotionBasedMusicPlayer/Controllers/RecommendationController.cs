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
using EmotionBasedMusicPlayer.Business.Utils;
using System.IO;

namespace EmotionBasedMusicPlayer.Controllers
{
    [AuthenticationFilter]
    [RoutePrefix("recommendations")]
    public class RecommendationController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public List<Recommendation> GetRecommendations()
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            HttpPostedFile postedFile = httpRequest.Files["Image"];

            if(postedFile == null)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error.MissingImage");
                throw new HttpResponseException(response);
            }

            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(postedFile.InputStream))
            {
                fileData = binaryReader.ReadBytes(postedFile.ContentLength);
            }

            return BusinessContext.GetRecommendations(fileData, BusinessContext.CurrentUser.UserID);
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
