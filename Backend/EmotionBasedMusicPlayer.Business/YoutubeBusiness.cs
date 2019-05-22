using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Business.Utils.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business
{
    public class YoutubeBusiness : BusinessObject
    {
        #region Members
        private readonly YoutubeClient _client;
        #endregion

        #region Constructors
        public YoutubeBusiness(BusinessContext context) : base(context)
        {
            _client = new YoutubeClient();
        }
        #endregion

        #region Methods
        public string GetVideoUrl(List<string> artists, string title)
        {
            string artistsString = string.Join("", artists);
            return _client.GetVideoUrl(artistsString, title);
        }
        #endregion
    }
}
