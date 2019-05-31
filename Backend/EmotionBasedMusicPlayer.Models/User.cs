using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class User
    {
        #region Properties
        [DataMember(Name = "UserID")]
        public Guid UserID { get; set; }

        [DataMember (Name ="Username")]
        public string Username { get; set; }

        [DataMember (Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Password")]
        public string Password { get; set; } 
        #endregion
    }
}
