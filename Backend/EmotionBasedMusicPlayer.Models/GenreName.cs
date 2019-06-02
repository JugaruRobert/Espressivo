using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class GenreName
    {
        #region Properties
        [DataMember(Name = "Name")]
        public string Name { get; set; } 
        #endregion
    }
}
