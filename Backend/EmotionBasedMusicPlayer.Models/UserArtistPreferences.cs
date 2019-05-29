using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class UserArtistPreferences
    {
        #region Properties
        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "Artists")]
        public List<Artist> Artists { get; set; }
        #endregion
    }
}
