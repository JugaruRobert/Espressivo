using EmotionBasedMusicPlayer.Business.Client;
using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Business.Models;
using EmotionBasedMusicPlayer.Business.UrlBuilder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business
{
    public class EmotionRecognitionBusiness : BusinessObject
    {
        #region Members
        private readonly EmotionRecognitionUrlBuilder _urlBuilder;
        private readonly EmotionRecognitionClient _client;
        #endregion

        #region Constructors
        public EmotionRecognitionBusiness(BusinessContext context) : base(context)
        {
            _urlBuilder = new EmotionRecognitionUrlBuilder();
            _client = new EmotionRecognitionClient();
        }
        #endregion

        #region Methods
        public FaceAttributes AnalyzeImage(ByteArrayContent image)
        {
            JArray emotionInformationArray = _client.GetFaceData(_urlBuilder.GetEmotionInformation(), image);
            FaceAttributes faceAttributes = new FaceAttributes();

            for(int i=0;i< emotionInformationArray.Count;i++)
            {
                JObject emotionInformationObject = JObject.Parse(emotionInformationArray[i].ToString());
                if (!emotionInformationObject.ContainsKey("faceAttributes"))
                    continue;
                faceAttributes = faceAttributes + JsonConvert.DeserializeObject<FaceAttributes>(emotionInformationObject["faceAttributes"].ToString());
            }

            return faceAttributes;
        }
        #endregion
    }
}
