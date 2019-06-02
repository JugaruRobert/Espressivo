using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business
{
    public class UserArtistBusiness : BusinessObject
    {
        #region Constructors
        public UserArtistBusiness(BusinessContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(UserArtistPreferences userArtists)
        {
            _context.DALContext.UserArtistDAL.DeleteByUserID(userArtists.UserID);
            foreach (Artist artist in userArtists.Artists)
            {
                Artist existingArtist = _context.DALContext.ArtistDAL.ReadByID(artist.ArtistID);
                if (existingArtist == null)
                    _context.DALContext.ArtistDAL.Insert(artist);

                _context.DALContext.UserArtistDAL.Insert(new UserArtist(){
                    UserID = userArtists.UserID,
                    ArtistID = artist.ArtistID
                });
            }
        }

        public void Delete(Guid userID, string artistID)
        {
            _context.DALContext.UserArtistDAL.Delete(userID, artistID);
        }

        public void DeleteByUserID(Guid userID)
        {
            _context.DALContext.UserArtistDAL.DeleteByUserID(userID);
        }

        public IEnumerable<UserArtist> ReadAll()
        {
            return _context.DALContext.UserArtistDAL.ReadAll();
        }

        public IEnumerable<Artist> ReadByUserID(Guid userID)
        {
            return _context.DALContext.UserArtistDAL.ReadByUserID(userID);
        }
        #endregion
    }
}
