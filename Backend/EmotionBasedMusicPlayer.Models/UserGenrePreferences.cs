﻿using System;
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
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public List<string> Genres { get; set; }
        #endregion
    }
}
