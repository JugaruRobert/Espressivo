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
    public class GenreDAL : DALObject
    {
        #region Constructor
        public GenreDAL(DALContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(Genre genre)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Genres_Insert", genre.GenerateSqlParametersFromModel().ToArray());
        }

        public void Update(Genre genre)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Genres_Update", genre.GenerateSqlParametersFromModel().ToArray());
        }

        public void Delete(Guid genreID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Genres_Remove", new SqlParameter("GenreID", genreID));
        }

        public void DeleteByName(string name)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Genres_RemoveByName", new SqlParameter("Name", name));
        }

        public IEnumerable<Genre> ReadAll()
        {
            return DbOperations.ExecuteQuery<Genre>(_context.connectionString, "dbo.Genres_ReadAll");
        }

        public Genre ReadByID(Guid genreID)
        {
            return DbOperations.ExecuteQuery<Genre>(_context.connectionString, "dbo.Genres_ReadByID", new SqlParameter("GenreID", genreID)).FirstOrDefault();
        }

        public Genre ReadByName(string name)
        {
            return DbOperations.ExecuteQuery<Genre>(_context.connectionString, "dbo.Genres_ReadByName", new SqlParameter("Name", name)).FirstOrDefault();
        }
        #endregion
    }
}
