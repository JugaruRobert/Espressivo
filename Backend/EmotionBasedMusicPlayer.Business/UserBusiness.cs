using EmotionBasedMusicPlayer.Business.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business
{
    public class UserBusiness : BusinessObject
    {
        #region Constructors
        public UserBusiness(BusinessContext context) : base(context) { }
        #endregion

        #region Methods
        public User ReadUser(String username, String password)
        {
            return _context.DALContext.UserDAL.ReadUser(username, password);
        }

        public void Insert(User user)
        {
            _context.DALContext.UserDAL.Insert(user);
        }

        public IEnumerable<User> ReadAll()
        {
            return _context.DALContext.UserDAL.ReadAll();
        }

        public User ReadByID(string username)
        {
            return _context.DALContext.UserDAL.ReadByID(username);
        }

        public User ReadByUsernameAndEmail(string username,string email)
        {
            return _context.DALContext.UserDAL.ReadByUsernameAndEmail(username,email);
        }

        public void Update(User user)
        {
            _context.DALContext.UserDAL.Update(user);
        }

        public void Delete(string username)
        {
            _context.DALContext.UserDAL.Delete(username);
        }
        #endregion
    }
}
