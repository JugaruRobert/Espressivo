using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models.Recommendations
{
    [DataContract]
    public class Recommendation
    {
        #region Properties
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public List<string> Artists { get; set; } = new List<string>();

        [DataMember]
        public List<AlbumImage> Images { get; set; } = new List<AlbumImage>();

        [DataMember]
        public string VideoID { get; set; }

        [DataMember]
        public string PredominantEmotion { get; set; }
        #endregion
    }
}
