using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Client
{
    public class EmotionRecognitionClient 
    {
        #region Methods
        public JArray GetFaceData(string url, ByteArrayContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                string subscriptionKey = ConfigurationManager.AppSettings["faceSubscriptionKey"];
                if (String.IsNullOrEmpty(subscriptionKey))
                    throw new InvalidOperationException("Face Subscription Key is missing for app configuration!");

                // Request headers.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // Get the JSON response.
                string stringResponse = response.Content.ReadAsStringAsync().Result;
                return JArray.Parse(stringResponse);
            }
        }
        #endregion
    }
}
