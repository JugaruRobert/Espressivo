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
    public class UserArtistDAL : DALObject
    {
        #region Constructor
        public UserArtistDAL(DALContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(UserArtist userArtist)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersArtists_Insert", userArtist.GenerateSqlParametersFromModel().ToArray());
        }

        public void Delete(Guid userID, string artistID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersArtists_Remove", new SqlParameter("UserID", userID), new SqlParameter("ArtistID", artistID));
        }

        public void DeleteByUserID(Guid userID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersArtists_RemoveByUserID", new SqlParameter("UserID", userID));
        }

        public IEnumerable<UserArtist> ReadAll()
        {
            return DbOperations.ExecuteQuery<UserArtist>(_context.connectionString, "dbo.UsersArtists_ReadAll");
        }

        public UserGenre ReadByUserID(Guid userID)
        {
            return DbOperations.ExecuteQuery<UserGenre>(_context.connectionString, "dbo.UsersArtists_ReadByUserID", new SqlParameter("UserID", userID)).FirstOrDefault();
        }
        #endregion
    }
}
