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
        public List<Recommendation> GetRecommendations(byte[] byteData, Guid userID)
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
                foreach (Artist seed in seeds)
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
                switch (predominantEmotion)
                {
                    case "happiness":
                        return GetHappinessTestRecommendations();
                    case "sadness":
                        return GetSadnessTestRecommendations();
                    case "surprise":
                        return GetHappinessTestRecommendations();
                    default:
                        return GetNeutralTestRecommendations();
                }
            return recommendations;
        } 

        public List<Recommendation> GetNeutralTestRecommendations()
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
                PredominantEmotion = "Neutral"
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
                PredominantEmotion = "Neutral"
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
                PredominantEmotion = "Neutral"
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
                PredominantEmotion = "Neutral"
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
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Hey, Soul Sister",
                Artists = new List<string>() { "Train" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/431f81b6b68c47ed68b172be81747c890ea0d111",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/741f4ab79ee89c9d554d27b2c15a646bb74319bd",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/52311f6de3003dfa4312276387facc44203f350f",
                        Width = 64
                    },
                },
                VideoID = "kVpv8-5XWOI",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Real",
                Artists = new List<string>() { "NF" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/555fec466ae1c4f3365d80c640b863a391b15da2",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/a9a3d82a6809119ab89171c2f16d30eea8f022d8",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/f705f35a796feb8692465205b2aeb044695a648a",
                        Width = 64
                    },
                },
                VideoID = "Po5zT1krKOc",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Counting Stars",
                Artists = new List<string>() { "OneRepublic" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/214301027935eb9b0836178b99a048052e3f6927",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/6115f636d1529d7fa3fbd15f0a60f0dd803e125f",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/9d8df8fe4c8ecca66982e3f97c7fa6b31b640a17",
                        Width = 64
                    },
                },
                VideoID = "hT_nvWreIhg",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "I'm Yours",
                Artists = new List<string>() { "Jason Mraz" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/678a0b275dc2a108cf88ad43897ab9e0a2875931",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/56b294a083b243e6a2ba3f9dd04588242fc8164a",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/013e793b16ec7244e5909ee67201ad945cf86974",
                        Width = 64
                    },
                },
                VideoID = "EkHTsc9PU2A",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Island In The Sun",
                Artists = new List<string>() { "Weezer" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/58a82347217a9a21f2bc4e6c91c3ac4e27a0a570",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/05ffd8a920d64ca44648308a4f13a05b90770f37",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/44ebe12912397a3eefdabac5c2de59f1f153a107",
                        Width = 64
                    },
                },
                VideoID = "erG5rgNYSdk",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "In The Lonely Hour",
                Artists = new List<string>() { "Sam Smith" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/49c34b3c9a2e7da5b1e847c21e8c0c883d78af77",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/88035902b20730411675224afaeef1c805536ed8",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/a89412b31dbadc9b4cead3663f409abdc14f7494",
                        Width = 64
                    },
                },
                VideoID = "UbNmMddxrOg",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Be Alright",
                Artists = new List<string>() { "Dean Lewis" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/294631ce16704eb6e6d63c8201bb07f43397ce69",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/9e1be389bfb3946c923f6888578b6a6f54c6b969",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/d60959e4c308849d29a217bbca82813efe23a882",
                        Width = 64
                    },
                },
                VideoID = "KRRzgV8CmAQ",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Heathens",
                Artists = new List<string>() { "Twenty One Pilots" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/413ebef976e2b4b4facf5013b6d323d3f5bd1ea5",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/6dba586fa6a4ade90ba58ff9346162d1e0d9fcf4",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/998d431d7d24af0e2c7f9f9414cc2cb8db151e82",
                        Width = 64
                    },
                },
                VideoID = "UprcpdwuwCg",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Fire",
                Artists = new List<string>() { "Gavin DeGraw" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/99cf07ac5ef44bbec9f9b0a200b99710f8958cb7",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/92bc928e62063640f901d00b886319a2f120afc2",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/5fb99e0f2dc27780f9713e30ac2c3b5bfc224ded",
                        Width = 64
                    },
                },
                VideoID = "sbbYPqgSe-M",
                PredominantEmotion = "Neutral"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "I See Fire",
                Artists = new List<string>() { "Ed Sheeran" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/c468b41d51f64f5b66b7c2633b4bdd890c7548d1",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/09013829af4fb1eb2553480dabf565941a79b9be",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/c856b79c0f3f70ed4a6942beaedb04389444bfe0",
                        Width = 64
                    },
                },
                VideoID = "2fngvQS_PmQ",
                PredominantEmotion = "Neutral"
            });

            return recommendations.OrderBy(arg => Guid.NewGuid()).Take(5).ToList();
        }

        public List<Recommendation> GetHappinessTestRecommendations()
        {
            List<Recommendation> recommendations = new List<Recommendation>();
            recommendations.Add(new Recommendation()
            {
                Title = "Doo-Wops & Hooligans",
                Artists = new List<string>() { "MAGIC!" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/0a01bbfac984fe3a55192aceea6c0fe9bf495c09",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/3d279adc8126da0e4b6f2247a6e1dbfbc559e1ac",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/96ebe0fc8c5c9a0ee315575935f1700ef8e02a1d",
                        Width = 64
                    },
                },
                VideoID = "PcsmsFsbwtM",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Oh, Happiness",
                Artists = new List<string>() { "David Crowder Band" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/f2e0ba619012b988a26b49ba468f126e6424161d",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/0aafc87da4abc5803fd12ea03afe97c0e1b17898",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/84cc388beca36436c5de36f16c875433aecd2149",
                        Width = 64
                    },
                },
                VideoID = "DTcThVJhDuM",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Rewrite The Stars",
                Artists = new List<string>() { "James Arthur", "Anne-Marie" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/d6b9f37838bb6f4eb8ffca96fe18b20b3d23f2f8",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/fe59f47047ff29fc36ad2ee01a50b657cc206c94",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/e2b730b0a50740808bf6ac5e1a4cb67670bf096a",
                        Width = 64
                    },
                },
                VideoID = "pRfmrE0ToTo",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "I'm A Believer",
                Artists = new List<string>() { "FRENSHIP", "Emily Warren" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/b6cbfdb8bd55b037bdc12e9d72e3868f01826014",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/511f43b3fbae39b8f16bc4837ffb6ccdaa1560d3",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/b5968eb68c3d3b9b989a1747ce809b83dd48d9e3",
                        Width = 64
                    },
                },
                VideoID = "0mYBSayCsH0",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "High Hopes",
                Artists = new List<string>() { "Panic! At The Disco" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/5a0c73915586db4a6acf0a92eb7c503877f1c9a4",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/c9c0ba55b658fcd567e2e1d71705fb24f617a2f6",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/94b61d7218ac0cd1436910bd6b1075e41789180d",
                        Width = 64
                    },
                },
                VideoID = "IPXIgEAGe4U",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Sweet but Psycho",
                Artists = new List<string>() { "Ava Max" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/9f92262af6d017302030d673f2826260b777edaf",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/8d1ffe98344be3e1b21cfda24b49cf1647d8c2da",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/e5696191b825a959f59fb8db8546d6ac87d573fe",
                        Width = 64
                    },
                },
                VideoID = "WXBHCQYxwr0",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Paradise",
                Artists = new List<string>() { "Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/e7a649b3890dc849e0f1597d6d12b4342e03ce5f",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/12cafa9559dc01eb0591a3cc2f526e7ed149d799",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/e187825d45a19dbf71320595318d2f057aa9b9df",
                        Width = 64
                    },
                },
                VideoID = "1G4isv_Fylg",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Adventure of a Lifetime",
                Artists = new List<string>() { "Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/2752509f00ef3cf46bf7fcb6433e3a3a631ed10c",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/9f31af5ca97f84f23a46ad152a89481668e6008f",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/16f43c30d41f88e6e7b7ad9c08003fc64dc27583",
                        Width = 64
                    },
                },
                VideoID = "QtXby3twMmI",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Price Tag",
                Artists = new List<string>() { "Jason Mraz" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/3d79a3640afd323d3f43950243bd39077afc4715",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/c62edba629678b7e1adabbad1833396d5c41fb0c",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/3090c8159fbb442379f90c336155bf1c401f8ecb",
                        Width = 64
                    },
                },
                VideoID = "qMxX-QOV9tI",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Drive By",
                Artists = new List<string>() { "Train" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/0b059563b98fe3b69ec710c750307960e15e511c",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/be95eea59f1ee35c5bab379c83729359966c7f12",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/5e21935dfcf68616fcce6a63df9d560ae6a82b53",
                        Width = 64
                    },
                },
                VideoID = "1agUFG0f50Y",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Something Just Like This",
                Artists = new List<string>() { "The Chainsmokers, Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/df3b17e748de56e4ce78ac29b216d3f99afd0c5a",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/ec1d389f4b448463b81513c0c5d5e1f6c34faa63",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/a2ec1de801d5a1e07123750e0216306a6e9c7d80",
                        Width = 64
                    },
                },
                VideoID = "FM7MFYoylVs",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "The Days",
                Artists = new List<string>() { "Avicii" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/3f09117997420e1e1b12c029127f87ba12a3e4b4",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/26d14cfe2e23cb6e06819de6048ecb5173645e73",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/6d7112b319bbb6ceafb28b09ab55551d0db4b382",
                        Width = 64
                    },
                },
                VideoID = "JDglMK9sgIQ",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Viva La Vida",
                Artists = new List<string>() { "Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/009aa1548af52b1d834648c6452f3804f086fead",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/a9e3493350233c5ee3f290b98d77664e83377982",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/7759ebd402ccb7ceec6618f5f0ab708359432f01",
                        Width = 64
                    },
                },
                VideoID = "dvgZkm1xWPE",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Your Song",
                Artists = new List<string>() { "Rita Ora" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/eadfad5a117899af07c82b4a05244ce64a9bd9e7",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/8b9f204b0c19f5874758da2ffc2a90ea86771d55",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/a819a787f9813518f30e198f205791547886cbf0",
                        Width = 64
                    },
                },
                VideoID = "i95Nlb7kiPo",
                PredominantEmotion = "Happiness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Streets",
                Artists = new List<string>() { "Kensington" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/6bdbef8ed3e3c201e9dfd637533879818d821a67",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/dfb6c7d4057907225b5c43da0412c76c8e00209a",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/41c15950b5f5f5b0bf44c33e38a14d1d46a3a954",
                        Width = 64
                    },
                },
                VideoID = "OoYbO46baX4",
                PredominantEmotion = "Happiness"
            });

            return recommendations.OrderBy(arg => Guid.NewGuid()).Take(5).ToList();
        }

        public List<Recommendation> GetSadnessTestRecommendations()
        {
            List<Recommendation> recommendations = new List<Recommendation>();
            recommendations.Add(new Recommendation()
            {
                Title = "Someone Like You",
                Artists = new List<string>() { "Adele" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/26007fcd7781ea9222b23ab3654ba86f60dd6e18",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/fe0f13e583408325f3b53118f632d386ac484813",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/22f4505ee7246c6e33c0e827b9764bdc968f8082",
                        Width = 64
                    },
                },
                VideoID = "hLQl3WQQoQ0",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Let It Go",
                Artists = new List<string>() { "James Bay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/116f1490cd76d53af12c7464dcf2400796d6f0e0",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/e7a80bedaa750dd78db39a767ff85e86f3908a67",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/1eae95416393668a91a4f807d45e898b9ed277d5",
                        Width = 64
                    },
                },
                VideoID = "GsPq9mzFNGY",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Midnight",
                Artists = new List<string>() { "Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/6218fc701d5c6e66c200f29b57fc4cd5f979313f",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/29ce98b4349b72c2778d2f82823159b06f98f8bc",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/4af48c1b31d3e2a2f6304226bb2e258e38541dbb",
                        Width = 64
                    },
                },
                VideoID = "BQeMxWjpr-Y",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Jealous",
                Artists = new List<string>() { "Labrinth" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/a8bfdddcca4dcc4c86184593a966f9561ba44c95",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/a76b4825d85d75d58067e323b9bf1a7fa4f2a797",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/c55e3ee6306b8a8485c78b0f73b8b3adbe16d991",
                        Width = 64
                    },
                },
                VideoID = "50VWOBi0VFs",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Sad Song",
                Artists = new List<string>() { "We The Kings", "Elena Coats" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/7f4eb1fac32a1208e59b18b8cf791fa74266c1b5",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/4caf10765c059d6498f5ea4816edfc8fedc83612",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/7895531269782db20f85cf65439f664c8a6a24ef",
                        Width = 64
                    },
                },
                VideoID = "BZsXcc_tC-o",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Half A Man",
                Artists = new List<string>() { "Dean Lewis" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/7eab5f3c02f5df9168f158a2d9d5e8688d177e6b",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/e8a935be20d24f3824fe97e67679ff73c8633073",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/20714ff9f0683d754e7fbd96eabd5c9889d81245",
                        Width = 64
                    },
                },
                VideoID = "Kua2dDhqzZw",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Time After Time",
                Artists = new List<string>() { "Eva Cassidy" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/d9ae99ab6f2febc1983050d99d7eb7ecfa1f9ec1",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/fc2870bb2fbfef5c3b955a9fce6b03c07e7580e5",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/a081a67d38d0c01cf77ee377c4a36f9e82b6f404",
                        Width = 64
                    },
                },
                VideoID = "KWvPOJOYqGA",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Mad World",
                Artists = new List<string>() { "Gary Jules", "Michael Andrews" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/c79127452b503305835a5f7cacc3aa59e9323e52",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/38a75ecc2b1e55f6d9f5ad5d50258983a84f5e48",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/89e999538eb9357d21bf926beddcaef9486684cf",
                        Width = 64
                    },
                },
                VideoID = "4N3N1MlvVc4",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "In My Head",
                Artists = new List<string>() { "Band of Horses" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/c87cf0f69990c0c175f97e1d4281b2eb576fbc77",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/d2a8ae920a61fc2b5476ca3d6896905a442f17b8",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/e36b4360497793554bdd4c1a893d275c10e9ea5f",
                        Width = 64
                    },
                },
                VideoID = "cMFWFhTFohk",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "O (Fly On)",
                Artists = new List<string>() { "Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/6218fc701d5c6e66c200f29b57fc4cd5f979313f",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/29ce98b4349b72c2778d2f82823159b06f98f8bc",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/4af48c1b31d3e2a2f6304226bb2e258e38541dbb",
                        Width = 64
                    },
                },
                VideoID = "eAbDNUj8pxo",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Thinking out Loud",
                Artists = new List<string>() { "Ed Sheeran" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/b68b39fdc2409d0f526ad48775ddcd93ff496cda",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/1f927459730fe3c72a2e9263975581a7e652e12f",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/109839612f24869cacfded433ae0d2a70a549923",
                        Width = 64
                    },
                },
                VideoID = "lp-EO5I60KA",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Yellow",
                Artists = new List<string>() { "Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/495b0549379fc4c324445fd7d2bfa219a8c18a90",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/94ac1cac6fcf2be17533ec6e7d638014b90642a2",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/68ff8a9ad0e1c8a7196ce5a3f7224b03cc3dec33",
                        Width = 64
                    },
                },
                VideoID = "yKNxeF4KMsY",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "All I Want",
                Artists = new List<string>() { "Kodaline" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/c367da14b3acaae08ec0b81c05fb64629da35c67",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/015861d173ebf206f27b5dd9ad94156a736a2637",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/392b14ff86f82edc3982a68a32cc1a0ff65f9c31",
                        Width = 64
                    },
                },
                VideoID = "n6BwAWiHcSg",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "If Tomorrow Never Comes",
                Artists = new List<string>() { "Ronan Keating" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/bfbf1677885bfd250cf30e253d93c07cb7d62dd3",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/fee0c0ca82931b6e13a7441d66ed0c92f00507d6",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/d37bb8bc1a3d3f271398bf57187f58c63db91c5d",
                        Width = 64
                    },
                },
                VideoID = "S4kzGhDEURA",
                PredominantEmotion = "Sadness"
            });

            recommendations.Add(new Recommendation()
            {
                Title = "Gravity",
                Artists = new List<string>() { "Coldplay" },
                Images = new List<AlbumImage>()
                {
                    new AlbumImage()
                    {
                        Height = 640,
                        Url = "https://i.scdn.co/image/72378d58fa992edc924f03d45e33019d40364ab4",
                        Width = 640
                    },
                    new AlbumImage()
                    {
                        Height = 300,
                        Url = "https://i.scdn.co/image/35505c9d962f0f5e2e268a5e3e377b0097e1de4a",
                        Width = 300
                    },
                    new AlbumImage()
                    {
                        Height = 64,
                        Url = "https://i.scdn.co/image/0c40fab952b47a5ef53d50641391affa46ee09f2",
                        Width = 64
                    },
                },
                VideoID = "emTePWNepkk",
                PredominantEmotion = "Sadness"
            });

            return recommendations.OrderBy(arg => Guid.NewGuid()).Take(5).ToList();
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
