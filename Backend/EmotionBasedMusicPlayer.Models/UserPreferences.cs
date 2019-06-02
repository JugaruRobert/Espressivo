using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class UserPreferences
    {
        #region Properties
        [DataMember]
        public IEnumerable<Artist> Artists { get; set; }

        [DataMember]
        public IEnumerable<GenreName> Genres { get; set; } 
        #endregion
    }
}
