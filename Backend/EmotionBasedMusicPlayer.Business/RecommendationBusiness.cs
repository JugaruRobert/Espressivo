using EmotionBasedMusicPlayer.Business.Models;
using EmotionBasedMusicPlayer.Business.Utils.recommendation;
using EmotionBasedMusicPlayer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Core
{
    public class RecommendationBusiness : BusinessObject
    {
        #region Constants
        private const string TOKEN_CACHE_KEY = "recommendationApi.BearerToken";
        #endregion
        
        #region Members
        private readonly ObjectCache _cache;
        private readonly RecommendationUrlBuilder _urlBuilder;
        private readonly RecommendationClient _client;
        #endregion

        #region Constructors
        public RecommendationBusiness(BusinessContext context) : base(context)
        {
            _cache = MemoryCache.Default;
            _urlBuilder = new RecommendationUrlBuilder();
            _client = new RecommendationClient();
        }
        #endregion

        #region Methods
        public string GetToken()
        {
            //check if token is saved in local cache
            var token = _cache == null ? null : (string)_cache.Get(TOKEN_CACHE_KEY);
            if (token != null)
                return token;

            //set credentials
            string clientID = ConfigurationManager.AppSettings["recommendationClientID"];
            string clientSecret = ConfigurationManager.AppSettings["recommendationClientSecret"];

            if (String.IsNullOrEmpty(clientID))
                throw new InvalidOperationException("recommendation ClientID is missing for app configuration!");

            if (String.IsNullOrEmpty(clientSecret))
                throw new InvalidOperationException("recommendation ClientSecret is missing for app configuration!");

            string credentials = String.Format("{0}:{1}", clientID, clientSecret);

            using (var client = new HttpClient())
            {
                //Define Headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

                //Prepare Request Body
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                //Request Token
                var currentTime = DateTime.Now;
                var request = client.PostAsync("https://accounts.spotify.com/api/token", requestBody).Result;
                var response = request.Content.ReadAsStringAsync().Result;
                dynamic tokenData = JsonConvert.DeserializeObject<AccessToken>(response);
                token = tokenData.access_token;

                //Save to cache
                if (_cache != null)
                    _cache.Add(TOKEN_CACHE_KEY, token, currentTime.AddSeconds(Convert.ToInt32(tokenData.expires_in)));
            }

            return token;
        }

        public List<string> GetGenreSeeds()
        {
            var token = GetToken();
            if (token == null || string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Invalid or missing access token!");

            JObject genresObject = _client.GetData(_urlBuilder.GetGenreSeeds(),token);
            if (!genresObject.ContainsKey("genres"))
                throw new Exception("Invalid json response! - Get seed genres!");

            return genresObject["genres"].Select(genre => (string)genre).ToList();
        }

        public JObject GetRecommendations(List<string> artistSeed = null, List<string> genreSeed = null,
            TuneableTrack target = null, TuneableTrack min = null, TuneableTrack max = null, int limit = 15, string market = "")
        {
            var token = GetToken();
            if (token == null || string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Invalid or missing access token!");

            return _client.GetData(_urlBuilder.GetRecommendations(artistSeed, genreSeed, target, min, max, limit, market), token);
        }

        public JObject GetArtistSeeds(string artistName)
        {
            var token = GetToken();
            if (token == null || string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Invalid or missing access token!");

            return _client.GetData(_urlBuilder.GetArtistSeeds(artistName), token);
        }
        #endregion
    }
}
