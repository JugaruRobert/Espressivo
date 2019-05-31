using EmotionBasedMusicPlayer.Models;
using PinguiniiGalactici.NewAcademicInfo.DAL.Core;
using PinguiniiGalactici.NewAcademicInfo.Library;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.DAL
{
    public class UserGenreDAL : DALObject
    {
        #region Constructor
        public UserGenreDAL(DALContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(UserGenre userGenre)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersGenres_Insert", userGenre.GenerateSqlParametersFromModel().ToArray());
        }

        public void Delete(Guid userID, Guid genreID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersGenres_Remove", new SqlParameter("UserID", userID), new SqlParameter("GenreID", genreID));
        }

        public void DeleteByUserID(Guid userID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersGenres_RemoveByUserID", new SqlParameter("UserID", userID));
        }

        public IEnumerable<UserGenre> ReadAll()
        {
            return DbOperations.ExecuteQuery<UserGenre>(_context.connectionString, "dbo.UsersGenres_ReadAll");
        }

        public UserGenre ReadByUserID(Guid userID)
        {
            return DbOperations.ExecuteQuery<UserGenre>(_context.connectionString, "dbo.UsersGenres_ReadByUserID", new SqlParameter("UserID", userID)).FirstOrDefault();
        }
        #endregion
    }
}
