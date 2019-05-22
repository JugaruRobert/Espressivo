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
        public float? Acousticness { get; set; }

        public float? Danceability { get; set; }

        public int? Duration_Ms { get; set; }

        public float? Energy { get; set; }

        public float? Instrumentalness { get; set; }

        public int? Key { get; set; }

        public float? Liveness { get; set; }

        public float? Loudness { get; set; }

        public int? Mode { get; set; }

        public int? Popularity { get; set; }

        public float? Speechiness { get; set; }

        public float? Tempo { get; set; }

        public int? Time_Signature { get; set; }

        public float? Valence { get; set; }
        #endregion

        #region Constructors
        public TuneableTrack()
        {

        }

        public TuneableTrack(FaceAttributes faceAttributes)
        {
            float happiness = (50 * faceAttributes.smile) / 100 + (50 * faceAttributes.emotion.happiness) / 100;
            Valence = 1;
            Energy = 1;
            Danceability = 1;
            Mode = 1;
            Popularity = 100;
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
