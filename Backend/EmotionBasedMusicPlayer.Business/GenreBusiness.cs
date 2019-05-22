using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business
{
    public class GenreBusiness : BusinessObject
    {
        #region Constructors
        public GenreBusiness(BusinessContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(Genre genre)
        {
            _context.DALContext.GenreDAL.Insert(genre);
        }

        public void Update(Genre genre)
        {
            _context.DALContext.GenreDAL.Update(genre);
        }

        public void Delete(Guid genreID)
        {
            _context.DALContext.GenreDAL.Delete(genreID);
        }

        public void DeleteByName(string name)
        {
            _context.DALContext.GenreDAL.DeleteByName(name);
        }

        public IEnumerable<Genre> ReadAll()
        {
            return _context.DALContext.GenreDAL.ReadAll();
        }

        public Genre ReadByID(Guid genreID)
        {
            return _context.DALContext.GenreDAL.ReadByID(genreID);
        }

        public Genre ReadByName(string name)
        {
            return _context.DALContext.GenreDAL.ReadByName(name);

        }
        #endregion
    }
}
