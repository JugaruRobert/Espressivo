using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models.Recommendations
{
    [DataContract]
    public class AlbumImage
    {
        #region Properties
        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public int Width { get; set; }
        #endregion
    }
}
