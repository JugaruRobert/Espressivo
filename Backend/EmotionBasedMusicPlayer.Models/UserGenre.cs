using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class UserGenre
    {
        #region Properties
        [DataMember(Name = "UserID")]
        public Guid UserID { get; set; }

        [DataMember(Name = "GenreID")]
        public Guid GenreID { get; set; } 
        #endregion
    }
}
