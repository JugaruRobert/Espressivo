using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Models
{
    [DataContract]
    public class FaceEmotions
    {
        #region Properties
        [DataMember]
        public float anger { get; set; }

        [DataMember]
        public float contempt { get; set; }

        [DataMember]
        public float disgust { get; set; }

        [DataMember]
        public float fear { get; set; }

        [DataMember]
        public float happiness { get; set; }

        [DataMember]
        public float neutral { get; set; }

        [DataMember]
        public float sadness { get; set; }

        [DataMember]
        public float surprise { get; set; }
        #endregion

        #region Constructors
        public FaceEmotions()
        {
            anger = 0;
            contempt = 0;
            disgust = 0;
            fear = 0;
            happiness = 0;
            neutral = 0;
            sadness = 0;
            surprise = 0;
        }
        #endregion

        #region Methods
        public static FaceEmotions operator +(FaceEmotions first, FaceEmotions second)
        {
            FaceEmotions faceEmotions = new FaceEmotions();

            faceEmotions.anger = (first.anger + second.anger) / 2;
            faceEmotions.contempt = (first.contempt + second.contempt) / 2;
            faceEmotions.disgust = (first.disgust + second.disgust) / 2;
            faceEmotions.fear = (first.fear + second.fear) / 2;
            faceEmotions.happiness = (first.happiness + second.happiness) / 2;
            faceEmotions.neutral = (first.neutral + second.neutral) / 2;
            faceEmotions.sadness = (first.sadness + second.sadness) / 2;
            faceEmotions.surprise = (first.surprise + second.surprise) / 2;

            return faceEmotions;
        }
        #endregion
    }
}
