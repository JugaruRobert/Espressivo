using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business
{
    public class UserGenreBusiness : BusinessObject
    {
        #region Constructors
        public UserGenreBusiness(BusinessContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(UserGenrePreferences userGenres)
        {
            _context.DALContext.UserGenreDAL.DeleteByUserID(userGenres.UserID);
            foreach(string genreName in userGenres.Genres)
            {
                Genre existingGenre = _context.DALContext.GenreDAL.ReadByName(genreName);
                Guid? genreID = existingGenre?.GenreID;
                if (existingGenre == null)
                {
                    genreID = Guid.NewGuid();
                    _context.DALContext.GenreDAL.Insert(new Genre(){
                        GenreID = genreID.Value,
                        Name = genreName
                    });
                }
                _context.DALContext.UserGenreDAL.Insert(new UserGenre(){
                    UserID = userGenres.UserID,
                    GenreID = genreID.Value
                });
            }
        }

        public void Delete(Guid userID, Guid genreID)
        {
            _context.DALContext.UserGenreDAL.Delete(userID, genreID);
        }

        public void DeleteByUserID(Guid userID)
        {
            _context.DALContext.UserGenreDAL.DeleteByUserID(userID);
        }

        public IEnumerable<UserGenre> ReadAll()
        {
            return _context.DALContext.UserGenreDAL.ReadAll();
        }

        public IEnumerable<GenreName> ReadByUserID(Guid userID)
        {
            return _context.DALContext.UserGenreDAL.ReadByUserID(userID);
        }
        #endregion
    }
}
