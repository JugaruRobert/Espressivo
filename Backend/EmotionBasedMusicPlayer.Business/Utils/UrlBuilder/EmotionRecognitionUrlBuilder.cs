using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.UrlBuilder
{
    public class EmotionRecognitionUrlBuilder
    {
        #region Constants
        public const string FaceAPIBase = "https://northeurope.api.cognitive.microsoft.com/face/v1.0//detect";
        #endregion

        #region Methods
        public string GetEmotionInformation()
        {
            return $"{FaceAPIBase}?returnFaceId=false&returnFaceLandmarks=false&returnFaceAttributes=age,gender,smile,emotion";
        }
        #endregion
    }
}
