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

        public void Delete(string username, string artistID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersArtists_Remove", new SqlParameter("Username", username), new SqlParameter("ArtistID", artistID));
        }

        public void DeleteByUsername(string username)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.UsersArtists_RemoveByUsername", new SqlParameter("Username", username));
        }

        public IEnumerable<UserArtist> ReadAll()
        {
            return DbOperations.ExecuteQuery<UserArtist>(_context.connectionString, "dbo.UsersArtists_ReadAll");
        }

        public UserGenre ReadByUsername(string name)
        {
            return DbOperations.ExecuteQuery<UserGenre>(_context.connectionString, "dbo.UsersArtists_ReadByUsername", new SqlParameter("Username", name)).FirstOrDefault();
        }
        #endregion
    }
}
