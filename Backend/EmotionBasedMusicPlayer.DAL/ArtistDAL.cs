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
    public class ArtistDAL : DALObject
    {
        #region Constructor
        public ArtistDAL(DALContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(Artist artist)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Artists_Insert", artist.GenerateSqlParametersFromModel().ToArray());
        }

        public void Update(Artist artist)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Artists_Update", artist.GenerateSqlParametersFromModel().ToArray());
        }

        public void Delete(string artistID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Artists_Remove", new SqlParameter("ArtistID", artistID));
        }

        public void DeleteByName(string name)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Artists_RemoveByName", new SqlParameter("Name", name));
        }

        public IEnumerable<Artist> ReadAll()
        {
            return DbOperations.ExecuteQuery<Artist>(_context.connectionString, "dbo.Artists_ReadAll");
        }

        public Artist ReadByID(string artistID)
        {
            return DbOperations.ExecuteQuery<Artist>(_context.connectionString, "dbo.Artists_ReadByID", new SqlParameter("ArtistID", artistID)).FirstOrDefault();
        }

        public Artist ReadByName(string name)
        {
            return DbOperations.ExecuteQuery<Artist>(_context.connectionString, "dbo.Artists_ReadByName", new SqlParameter("Name", name)).FirstOrDefault();
        }
        #endregion
    }
}
