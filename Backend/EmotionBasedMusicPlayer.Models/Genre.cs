using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Models
{
    [DataContract]
    public class Genre
    {
        #region Properties
        [DataMember(Name="GenreID")]
        public Guid GenreID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; } 
        #endregion
    }
}
