using EmotionBasedMusicPlayer.Business.Models;
using EmotionBasedMusicPlayer.Business.Utils;
using EmotionBasedMusicPlayer.Models;
using EmotionBasedMusicPlayer.Models.Recommendations;
using Newtonsoft.Json.Linq;
using PinguiniiGalactici.NewAcademicInfo.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Core
{
    public class BusinessContext : IDisposable
    {
        #region Members
        private User _currentUser;
        private DALContext _dalContext;
        private RecommendationBusiness _recommendationBusiness;
        private EmotionRecognitionBusiness _emotionRecognitionBusiness;
        private YoutubeBusiness _youtubeBusiness;
        private ArtistBusiness _artistBusiness;
        private GenreBusiness _genreBusiness;
        private UserBusiness _userBusiness;
        private UserArtistBusiness _userArtistBusiness;
        private UserGenreBuisness _userGenreBuisness;
        #endregion

        #region Constructor
        public BusinessContext() { }

        public BusinessContext(User authenticatedUser)
        {
            _currentUser = authenticatedUser;
        }
        #endregion

        #region Properties
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }

        public DALContext DALContext
        {
            get
            {
                if (_dalContext == null)
                {
                    _dalContext = new DALContext();
                }
                return _dalContext;
            }
        }

        public RecommendationBusiness RecommendationBusiness
        {
            get
            {
                if (_recommendationBusiness == null)
                {
                    _recommendationBusiness = new RecommendationBusiness(this);
                }
                return _recommendationBusiness;
            }
        }

        public YoutubeBusiness YoutubeBusiness
        {
            get
            {
                if (_youtubeBusiness == null)
                {
                    _youtubeBusiness = new YoutubeBusiness(this);
                }
                return _youtubeBusiness;
            }
        }

        public EmotionRecognitionBusiness EmotionRecognitionBusiness
        {
            get
            {
                if (_emotionRecognitionBusiness == null)
                {
                    _emotionRecognitionBusiness = new EmotionRecognitionBusiness(this);
                }
                return _emotionRecognitionBusiness;
            }
        }

        public ArtistBusiness ArtistBusiness
        {
            get
            {
                if (_artistBusiness == null)
                {
                    _artistBusiness = new ArtistBusiness(this);
                }
                return _artistBusiness;
            }
        }

        public GenreBusiness GenreBusiness
        {
            get
            {
                if (_genreBusiness == null)
                {
                    _genreBusiness = new GenreBusiness(this);
                }
                return _genreBusiness;
            }
        }

        public UserBusiness UserBusiness
        {
            get
            {
                if (_userBusiness == null)
                {
                    _userBusiness = new UserBusiness(this);
                }
                return _userBusiness;
            }
        }

        public UserArtistBusiness UserArtistBusiness
        {
            get
            {
                if (_userArtistBusiness == null)
                {
                    _userArtistBusiness = new UserArtistBusiness(this);
                }
                return _userArtistBusiness;
            }
        }

        public UserGenreBuisness UserGenreBuisness
        {
            get
            {
                if (_userGenreBuisness == null)
                {
                    _userGenreBuisness = new UserGenreBuisness(this);
                }
                return _userGenreBuisness;
            }
        }
        #endregion

        #region Methods
        public List<Recommendation> GetRecommendations()
        {
            string imagePath = "C:\\Users\\Robert\\Desktop\\2.jpg";
            byte[] byteData = ImageUtils.GetImageAsByteArray(imagePath);

            FaceAttributes emotionData = EmotionRecognitionBusiness.AnalyzeImage(new ByteArrayContent(byteData));
            TuneableTrack track = new TuneableTrack(emotionData);
            JObject recommendationsJSON = RecommendationBusiness.GetRecommendations(genreSeed: new List<string>() { "pop" });
            return GetVideoUrls(recommendationsJSON);
        }

        public List<Recommendation> GetVideoUrls(JObject recommendationsJSON)
        {
            List<Recommendation> recommendations = new List<Recommendation>();
            foreach(var track in recommendationsJSON.SelectToken("tracks"))
            {
                Recommendation recommendation = new Recommendation();

                string title = track.SelectToken("name")?.ToString();
                if (String.IsNullOrEmpty(title))
                    continue;
                recommendation.Title = title;

                foreach (var artist in track.SelectToken("artists"))
                {
                    string name = artist.SelectToken("name")?.ToString();
                    if (String.IsNullOrEmpty(name))
                        continue;
                    recommendation.Artists.Add(name);
                }

                recommendation.Url = YoutubeBusiness.GetVideoUrl(recommendation.Artists,recommendation.Title);
                recommendations.Add(recommendation);
            }
            return recommendations;
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            DisposeBusinessObject(_recommendationBusiness);
            DisposeBusinessObject(_emotionRecognitionBusiness);
            DisposeBusinessObject(_youtubeBusiness);
            DisposeBusinessObject(_artistBusiness);
            DisposeBusinessObject(_genreBusiness);
            DisposeBusinessObject(_userBusiness);
            DisposeBusinessObject(_userArtistBusiness);
            DisposeBusinessObject(_userGenreBuisness);

            if (_dalContext != null)
            {
                _dalContext.Dispose();
                _dalContext = null;
            }
        }

        private void DisposeBusinessObject(BusinessObject businessObject)
        {
            if (businessObject != null)
            {
                businessObject.Dispose();
                businessObject = null;
            }
        }

        ~BusinessContext()
        {
            Dispose(false);
        }
        #endregion
    }
}
