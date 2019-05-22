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

        public void Delete(string username, Guid genreID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersGenres_Remove", new SqlParameter("Username", username), new SqlParameter("GenreID", genreID));
        }

        public void DeleteByUsername(string username)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersGenres_RemoveByUsername", new SqlParameter("Username", username));
        }

        public IEnumerable<UserArtist> ReadAll()
        {
            return DbOperations.ExecuteQuery<UserArtist>(_context.connectionString, "dbo.UsersGenres_ReadAll");
        }

        public UserGenre ReadByUsername(string name)
        {
            return DbOperations.ExecuteQuery<UserGenre>(_context.connectionString, "dbo.UsersGenres_ReadByUsername", new SqlParameter("Username", name)).FirstOrDefault();
        }
        #endregion
    }
}
