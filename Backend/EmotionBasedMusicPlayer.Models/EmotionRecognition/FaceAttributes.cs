using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Models
{
    [DataContract]
    public class FaceAttributes
    {
        #region Properties
        [DataMember]
        public float smile { get; set; }

        [DataMember]
        public float age { get; set; }

        [DataMember]
        public string gender { get; set; }

        [DataMember]
        public FaceEmotions emotion { get; set; }
        #endregion

        #region Constructors
        public FaceAttributes()
        {
            smile = 0;
            age = 0;
            gender = String.Empty;
            emotion = new FaceEmotions();
        }
        #endregion

        #region Methods
        public static FaceAttributes operator +(FaceAttributes first, FaceAttributes second)
        {
            FaceAttributes faceAttributes = new FaceAttributes();

            faceAttributes.smile = (first.smile + second.smile) / 2;
            faceAttributes.age = (first.age + second.age) / 2;

            if (first.gender == String.Empty)
                faceAttributes.gender = second.gender;
            else if (first.gender.ToLower() != second.gender.ToLower())
                faceAttributes.gender = String.Empty;

            faceAttributes.emotion = first.emotion + second.emotion;

            return faceAttributes;
        }

        public string GetPredominantEmotion()
        {
            PropertyInfo property = emotion.GetType().GetProperties().Aggregate((p1, p2) =>
                (float)p1.GetValue(emotion) > (float)p2.GetValue(emotion) ? p1 : p2
            );

            if ((float)property.GetValue(emotion) == 0)
                return "neutral";
            return property.Name;
        }
        #endregion
    }
}
