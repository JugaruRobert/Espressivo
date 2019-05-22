using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class AccessToken
    {
        #region Properties
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public long expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
        #endregion
    }
}