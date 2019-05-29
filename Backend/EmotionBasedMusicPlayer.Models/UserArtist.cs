using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class UserArtist
    {
        #region Properties
        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "ArtistID")]
        public string ArtistID { get; set; } 
        #endregion
    }
}
