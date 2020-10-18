using System.Collections.Generic;

namespace TennisApplication.Repository.User
{
    public interface IUserRepository
    {
        bool SaveChanges();
        void CreateUser(Models.User user);
        IEnumerable<Models.User> GetAllUsers();
        Models.User GetUserById(int id);
        int FindIdByEMailAndPassword(string eMail, string password);
    }
}