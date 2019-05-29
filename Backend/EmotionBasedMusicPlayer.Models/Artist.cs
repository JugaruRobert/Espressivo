using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class Artist
    {
        #region Properties
        [DataMember(Name = "ArtistID")]
        public string ArtistID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; } 
        #endregion
    }
}
