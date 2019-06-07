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
    public class UserDAL : DALObject
    {
        #region Constructor
        public UserDAL(DALContext context) : base(context) { }
        #endregion

        #region Methods
        public void Insert(User user)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Users_Insert", user.GenerateSqlParametersFromModel().ToArray());
        }

        public void Update(Guid userID, string username, string email)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Users_Update", new SqlParameter("UserID", userID),
                                                                                       new SqlParameter("Username", username),
                                                                                       new SqlParameter("Email", email));
        }

        public void DeleteByUsername(string username)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Users_RemoveByUsername", new SqlParameter("Username", username));
        }

        public void DeleteByID(Guid userID)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Users_RemoveByID", new SqlParameter("UserID", userID));
        }

        public IEnumerable<User> ReadAll()
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_ReadAll");
        }

        public User ReadUser(string username,string password)
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_Read", new SqlParameter("Username", username), new SqlParameter("Password", password)).FirstOrDefault();
        }

        public User ReadByUsernameOrEmail(Guid userID,string username, string email)
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_ReadByUsernameOrEmail", new SqlParameter("UserID", userID),
                                                                                                                 new SqlParameter("Username", username), 
                                                                                                                 new SqlParameter("Email", email)).FirstOrDefault();
        }

        public IEnumerable<Artist> ReadUserPreferences(Guid userID)
        {
            return DbOperations.ExecuteQuery<Artist>(_context.connectionString, "dbo.GetRandomSeeds", new SqlParameter("UserID", userID));
        }

        public User ReadByUsername(string username)
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_ReadByUsername", new SqlParameter("Username", username)).FirstOrDefault();
        }

        public User ReadByID(Guid userID)
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_ReadByID", new SqlParameter("UserID", userID)).FirstOrDefault();
        }
        #endregion
    }
}
