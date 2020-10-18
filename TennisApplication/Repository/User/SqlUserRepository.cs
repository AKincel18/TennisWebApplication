using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using TennisApplication.Database;
using TennisApplication.Models;
using TennisApplication.Repository.User;

namespace TennisApplication.Repository.User
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void CreateUser(Models.User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public IEnumerable<Models.User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public Models.User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(t => t.Id == id);
        }

        public int FindIdByEMailAndPassword(string eMail, string password)
        {
            var foundUser = _context.Users.Where(user => user.EMail == eMail).FirstOrDefault(user => user.Password == password);
            return foundUser?.Id ?? -1; //(foundUser != null) ? foundUser.Id : -1;
        }
    }
}