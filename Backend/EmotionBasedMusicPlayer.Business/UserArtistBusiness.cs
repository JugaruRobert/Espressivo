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
            _context.DALContext.UserArtistDAL.DeleteByUsername(userArtists.Username);
            foreach (Artist artist in userArtists.Artists)
            {
                Artist existingArtist = _context.DALContext.ArtistDAL.ReadByID(artist.ArtistID);
                if (existingArtist == null)
                    _context.DALContext.ArtistDAL.Insert(artist);

                _context.DALContext.UserArtistDAL.Insert(new UserArtist(){
                    Username = userArtists.Username,
                    ArtistID = artist.ArtistID
                });
            }
        }

        public void Delete(string username, string artistID)
        {
            _context.DALContext.UserArtistDAL.Delete(username, artistID);
        }

        public void DeleteByUsername(string username)
        {
            _context.DALContext.UserArtistDAL.DeleteByUsername(username);
        }

        public IEnumerable<UserArtist> ReadAll()
        {
            return _context.DALContext.UserArtistDAL.ReadAll();
        }

        public UserGenre ReadByUsername(string name)
        {
            return _context.DALContext.UserArtistDAL.ReadByUsername(name);
        }
        #endregion
    }
}
