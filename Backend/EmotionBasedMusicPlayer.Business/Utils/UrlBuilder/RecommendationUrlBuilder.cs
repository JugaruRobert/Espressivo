using EmotionBasedMusicPlayer.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Utils.recommendation
{
    public class RecommendationUrlBuilder
    {
        #region Constants
        public const string spotifyAPIBase = "https://api.spotify.com/v1";
        #endregion

        #region Methods
        public string GetRecommendations(List<string> artistSeed = null, List<string> genreSeed = null,
                                         TuneableTrack target = null, TuneableTrack min = null, TuneableTrack max = null, int limit = 20, string market = "")
        {
            limit = Math.Min(100, limit);
            StringBuilder builder = new StringBuilder($"{spotifyAPIBase}/recommendations");
            builder.Append("?limit=" + limit);
            if (artistSeed?.Count > 0)
                builder.Append("&seed_artists=" + string.Join(",", artistSeed));
            if (genreSeed?.Count > 0)
                builder.Append("&seed_genres=" + string.Join(",", genreSeed));
            if (target != null)
                builder.Append(target.BuildUrl("target"));
            if (min != null)
                builder.Append(min.BuildUrl("min"));
            if (max != null)
                builder.Append(max.BuildUrl("max"));
            if (!string.IsNullOrEmpty(market))
                builder.Append("&market=" + market);
            return builder.ToString();
        }

        public string GetGenreSeeds()
        {
            return $"{spotifyAPIBase}/recommendations/available-genre-seeds";
        }

        public string GetArtistSeeds(string artistName)
        {
            StringBuilder builder = new StringBuilder($"{spotifyAPIBase}/search");
            builder.Append("?q=" + artistName.ToLower());
            builder.Append("&type=artist");
            return builder.ToString();
        }
        #endregion
    }
}
