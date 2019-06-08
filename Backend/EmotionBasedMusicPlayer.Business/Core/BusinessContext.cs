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
        private UserGenreBusiness _userGenreBusiness;
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

        public UserGenreBusiness UserGenreBusiness
        {
            get
            {
                if (_userGenreBusiness == null)
                {
                    _userGenreBusiness = new UserGenreBusiness(this);
                }
                return _userGenreBusiness;
            }
        }
        #endregion

        #region Methods
        public List<Recommendation> GetRecommendations(byte[] byteData,Guid userID)
        {
            //string imagePath = "C:\\Users\\Robert\\Desktop\\2.jpg";
            //byte[] byteData = ImageUtils.GetImageAsByteArray(imagePath);

            FaceAttributes emotionData = EmotionRecognitionBusiness.AnalyzeImage(new ByteArrayContent(byteData));
            string predominantEmotion = emotionData.GetPredominantEmotion();
            TuneableTrack track = new TuneableTrack(emotionData);

            IEnumerable<Artist> seeds = DALContext.UserDAL.ReadUserPreferences(userID);
            List<string> genreSeeds = new List<string>();
            List<string> artistSeeds = new List<string>();

            if (seeds.Count() > 0)
            {
                foreach(Artist seed in seeds)
                {
                    if (seed.ArtistID == null)
                        genreSeeds.Add(seed.Name.ToLower());
                    else
                        artistSeeds.Add(seed.ArtistID);
                }
            }

            JObject recommendationsJSON = RecommendationBusiness.GetRecommendations(
                genreSeed: genreSeeds.Count > 0 ? genreSeeds : null,
                artistSeed: artistSeeds.Count > 0 ? artistSeeds : null);

            return GetVideoUrls(recommendationsJSON, userID, predominantEmotion);

            //return GetTestRecommendations();
        }

        public List<Recommendation> GetVideoUrls(JObject recommendationsJSON,Guid userID,string predominantEmotion)
        {
            List<Recommendation> recommendations = new List<Recommendation>();
            try
            {
                foreach (var track in recommendationsJSON.SelectToken("tracks"))
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

                    foreach (var image in track.SelectToken("album.images"))
                    {
                        recommendation.Images.Add(new AlbumImage()
                        {
                            Height = int.Parse(image.SelectToken("height").ToString()),
                            Url = image.SelectToken("url").ToString(),
                            Width = int.Parse(image.SelectToken("width").ToString())
                        });
                    }
                    recommendation.VideoID = YoutubeBusiness.GetVideoUrl(recommendation.Artists, recommendation.Title);                         
                    recommendation.PredominantEmotion = predominantEmotion;
                    recommendations.Add(recommendation);
                }
            }
            catch(Exception ex)
            {
                IEnumerable<GenreName> userGenreSeeds = DALContext.UserGenreDAL.ReadGenreSeeds(userID);
                List<string> genreSeeds = new List<string>();
                foreach (GenreName genreName in userGenreSeeds)
                    genreSeeds.Add(genreName.Name);
                recommendationsJSON = RecommendationBusiness.GetRecommendations(genreSeed: genreSeeds.Count() > 0 ? genreSeeds : null);
            }

            if (recommendations.Count == 0)
                return GetTestRecommendations();
            return recommendations;
        }

        public List<Recommendation> GetTestRecommendations()
        {
            List<Recommendation> recommendations = new List<Recommendation>();
            recommendations.Add(new Recommendation()
            {
                Title = "You Don't Know Love",
                Artists = new List<string>() { "Olly Murs" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/8de58e8d4564f29234bfbdb0daf9222c1c5e6c77",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/c7841d3e03dc7c0fc5f2d557e4a29a8251305abe",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/f369d93f08d9744cff7a9978e8747f8ad262c745",
                        Width = 64
                    },
                },
                VideoID = "VzN5tKIh7xE",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Take Me Home",
                Artists = new List<string>() { "Jess Glynne" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/64a7ae4a4804ae88f3b93af220c2eff8ee9b2882",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/bae3fdea03d703ec0e06b5a584d17a6af8f3ed1a",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/1c7c821199e9ca963108f6d7b99dede10b19605e",
                        Width = 64
                    },
                },
                VideoID = "2ebfSItB0oM",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Unsteady",
                Artists = new List<string>() { "X Ambassadors" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/3c90b0e40fd36f8060dbbf2b7f1eb3daacb197d5",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/d525ee687f6d2b62082f52bbe9953a0339f9b2c7",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/cbb1b57fc7af9f9029b2d7568e2bfd9ea1b42afd",
                        Width = 64
                    },
                },
                VideoID = "lMSakueE0Hw",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Capsize",
                Artists = new List<string>() { "FRENSHIP", "Emily Warren" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/509fde2dff5614b27f3a69b340750983b6ce6272",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/bbb8a896a70f056fc3e10ddcbd24b8c450275214",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/b3af239761e8448db05a314d25f475b16284a635",
                        Width = 64
                    },
                },
                VideoID = "2pY_WobtVuQ",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "When I Was Your Man",
                Artists = new List<string>() { "Bruno Mars" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/da39a8f80539ea048011ee3aa59f4066659290ea",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/e404a30a9e9a449e1f30156940fb0af63e45a42e",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/6f958e8a9335e1458fc81f22a72f87a11737179e",
                        Width = 64
                    },
                },
                VideoID = "ekzHIouo8Q4",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Angels",
                Artists = new List<string>() { "Khalid" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/5026fddc6c0b69e931ae99130f15d4214f1e7d41",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/3f2cc8df9f891830695c00446ce86bd892e60886",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/ee9853bb9b13357f12b691eaeac8b9a44c5c99d8",
                        Width = 64
                    },
                },
                VideoID = "V2CP7DoECnc",
                PredominantEmotion = "Happiness"
            });
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
            DisposeBusinessObject(_userGenreBusiness);

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
