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

        public void Update(User user)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Users_Update", user.GenerateSqlParametersFromModel().ToArray());
        }

        public void Delete(string username)
        {
            DbOperations.ExecuteCommand(_context.connectionString, "dbo.Users_Remove", new SqlParameter("Username", username));
        }

        public IEnumerable<User> ReadAll()
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_ReadAll");
        }

        public User ReadUser(string username,string password)
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_Read", new SqlParameter("Username", username), new SqlParameter("Password", password)).FirstOrDefault();
        }

        public User ReadByUsernameAndEmail(string username, string email)
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_ReadByUsernameAndEmail", new SqlParameter("Username", username), new SqlParameter("Email", email)).FirstOrDefault();
        }

        public User ReadByID(string username)
        {
            return DbOperations.ExecuteQuery<User>(_context.connectionString, "dbo.Users_ReadByID", new SqlParameter("Username", username)).FirstOrDefault();
        }
        #endregion
    }
}
