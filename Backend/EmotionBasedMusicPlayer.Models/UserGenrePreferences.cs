using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class UserGenrePreferences
    {
        #region Properties
        [DataMember(Name = "UserID")]
        public Guid UserID { get; set; }

        [DataMember(Name = "Genres")]
        public List<string> Genres { get; set; }
        #endregion
    }
}
