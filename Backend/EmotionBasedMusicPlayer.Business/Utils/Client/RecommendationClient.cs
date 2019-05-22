using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Utils.recommendation
{
    public class RecommendationClient
    {
        #region Methods
        public JObject GetData(string url, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                //Define Headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Execute the REST API call.
                HttpResponseMessage request = client.GetAsync(url).Result;

                // Get the JSON response.
                string response = request.Content.ReadAsStringAsync().Result;

                return JObject.Parse(response);
            }
        } 
        #endregion
    }
}
