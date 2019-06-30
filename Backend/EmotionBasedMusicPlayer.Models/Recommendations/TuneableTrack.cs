using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Models
{
    public class TuneableTrack
    {
        #region Properties
        public double? Acousticness { get; set; }

        public double? Danceability { get; set; }

        public int? Duration_Ms { get; set; }

        public double? Energy { get; set; }

        public double? Instrumentalness { get; set; }

        public int? Key { get; set; }

        public double? Liveness { get; set; }

        public double? Loudness { get; set; }

        public int? Mode { get; set; }

        public int? Popularity { get; set; }

        public double? Speechiness { get; set; }

        public double? Tempo { get; set; }

        public int? Time_Signature { get; set; }

        public double? Valence { get; set; }
        #endregion

        #region Constructors
        public TuneableTrack()
        {

        }

        public TuneableTrack(FaceAttributes faceAttributes)
        {
            Random random = new Random();
            string emotion = faceAttributes.GetPredominantEmotion();
            switch(emotion)
            {
                case "neutral":
                    Valence = random.NextDouble() * 0.2 + 0.4;
                    Energy = random.NextDouble() * 0.2 + 0.4;
                    Danceability = random.NextDouble() * 0.2 + 0.4;
                    Loudness = random.NextDouble() * 0.2 + 0.4;
                    Mode = random.Next(0, 2);
                    Tempo = random.Next(95, 116);
                    break;
                case "happiness":
                    Valence = 1;
                    Energy = 1;
                    Danceability = 1;
                    Loudness = 1;
                    Mode = 1;
                    Tempo = random.Next(120, 201);
                    break;
                case "anger":
                    Valence = random.NextDouble() * 0.3 + 0.2;
                    Energy = random.NextDouble() * 0.4 + 0.6;
                    Danceability = random.NextDouble() * 0.3 + 0.6;
                    Loudness = random.NextDouble() * 0.2 + 0.8;
                    Mode = random.Next(0, 2);
                    Tempo = random.Next(120, 201);
                    break;
                case "contempt":
                    Valence = random.NextDouble() * 0.3 + 0.1;
                    Energy = random.NextDouble() * 0.4 + 0.2;
                    Danceability = random.NextDouble() * 0.3 + 0.2;
                    Loudness = random.NextDouble() * 0.4 + 0.3;
                    Mode = 0;
                    Tempo = random.Next(85, 111);
                    break;
                case "fear":
                    Valence = random.NextDouble() * 0.2 + 0;
                    Energy = random.NextDouble() * 0.2 + 0.1;
                    Danceability = random.NextDouble() * 0.2 + 0.25;
                    Loudness = random.NextDouble() * 0.2 + 0.65;
                    Mode = random.Next(0, 2);
                    Tempo = random.Next(115, 136);
                    break;
                case "disgust":
                    Valence = random.NextDouble() * 0.2 + 0.3;
                    Energy = random.NextDouble() * 0.2 + 0.75;
                    Danceability = random.NextDouble() * 0.2 + 0.4;
                    Loudness = random.NextDouble() * 0.2 + 0.7;
                    Mode = random.Next(0, 2);
                    Tempo = random.Next(85, 131);
                    break;
                case "sadness":
                    Valence = 0;
                    Energy = 0;
                    Danceability = 0;
                    Loudness = 0;
                    Mode = 0;
                    Tempo = random.Next(80, 111);
                    break;
                case "surprise":
                    Valence = random.NextDouble() * 0.2 + 0.8;
                    Energy = random.NextDouble() * 0.2 + 0.8;
                    Danceability = random.NextDouble() * 0.4 + 0.6;
                    Loudness = random.NextDouble() * 0.6 + 0.4;
                    Mode = random.Next(0, 2);
                    Tempo = random.Next(110, 201);
                    break;
            }

            Loudness = 1 - Loudness * (-60);
        } 
        #endregion

        #region Methods
        public string BuildUrl(string prefix)
        {
            List<string> urlParams = new List<string>();
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object value = propertyInfo.GetValue(this);
                string name = propertyInfo.Name.ToLower();
                if (name == null || value == null)
                    continue;
                urlParams.Add(value is float valueAsFloat
                    ? $"{prefix}_{name}={valueAsFloat.ToString(CultureInfo.InvariantCulture)}"
                    : $"{prefix}_{name}={value}");
            }
            if (urlParams.Count > 0)
                return "&" + string.Join("&", urlParams);
            return "";
        }
        #endregion
    }
}
