using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business
{
    public class ArtistBusiness : BusinessObject
    {
        #region Constructors
        public ArtistBusiness(BusinessContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(Artist artist)
        {
            _context.DALContext.ArtistDAL.Insert(artist);
        }

        public void Update(Artist artist)
        {
            _context.DALContext.ArtistDAL.Update(artist);
        }

        public void Delete(string artistID)
        {
            _context.DALContext.ArtistDAL.Delete(artistID);
        }

        public void DeleteByName(string name)
        {
            _context.DALContext.ArtistDAL.DeleteByName(name);
        }

        public IEnumerable<Artist> ReadAll()
        {
            return _context.DALContext.ArtistDAL.ReadAll();
        }

        public Artist ReadByID(string artistID)
        {
            return _context.DALContext.ArtistDAL.ReadByID(artistID);
        }

        public Artist ReadByName(string name)
        {
            return _context.DALContext.ArtistDAL.ReadByName(name);
        }
        #endregion
    }
}
