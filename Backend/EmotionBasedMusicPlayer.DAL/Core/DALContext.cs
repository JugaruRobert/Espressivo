using EmotionBasedMusicPlayer.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinguiniiGalactici.NewAcademicInfo.DAL.Core
{
    public class DALContext : IDisposable
    {
        #region Members
        internal string connectionString;
        private UserDAL _userDAL;
        private ArtistDAL _artistDAL;
        private GenreDAL _genreDAL;
        private UserGenreDAL _userGenreDAL;
        private UserArtistDAL _userArtistDAL;
        #endregion

        #region Constructors
        public DALContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
        #endregion

        #region Properties      
        public UserDAL UserDAL
        {
            get
            {
                if (_userDAL == null)
                    _userDAL = new UserDAL(this);
                return _userDAL;
            }
        }

        public GenreDAL GenreDAL
        {
            get
            {
                if (_genreDAL == null)
                    _genreDAL = new GenreDAL(this);
                return _genreDAL;
            }
        }

        public ArtistDAL ArtistDAL
        {
            get
            {
                if (_artistDAL == null)
                    _artistDAL = new ArtistDAL(this);
                return _artistDAL;
            }
        }

        public UserArtistDAL UserArtistDAL
        {
            get
            {
                if (_userArtistDAL == null)
                    _userArtistDAL = new UserArtistDAL(this);
                return _userArtistDAL;
            }
        }

        public UserGenreDAL UserGenreDAL
        {
            get
            {
                if (_userGenreDAL == null)
                    _userGenreDAL = new UserGenreDAL(this);
                return _userGenreDAL;
            }
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

            DisposeDALObject(_userDAL);
            DisposeDALObject(_artistDAL);
            DisposeDALObject(_genreDAL);
            DisposeDALObject(_userArtistDAL);
            DisposeDALObject(_userGenreDAL);
        }

        private void DisposeDALObject(DALObject dalObject)
        {
            if (dalObject != null)
            {
                dalObject.Dispose();
                dalObject = null;
            }
        }

        ~DALContext()
        {
            Dispose(false);
        }
        #endregion
    }
}
