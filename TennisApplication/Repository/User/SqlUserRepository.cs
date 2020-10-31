using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public List<Models.User> GetUsersByTournament(int tournamentId)
        {
            //todo change this method
            var enrolments = _context.Enrolments.Where(t => t.Tournament.Id == tournamentId);
            var allUsers = _context.Users.ToList();
            var users = new List<Models.User>();
            foreach (var enrolment in enrolments)
            {
                foreach (var user in allUsers)
                {
                    if (user.Id == enrolment.User.Id)
                    {
                        users.Add(user);
                    }
                }

            }
            return users;
        }

        public bool IsUserRegisteredForTournamentById(int userId, int tournamentId)
        {
            return _context.Enrolments.Any(e => e.Tournament.Id == tournamentId && e.User.Id == userId);
        }

        public Models.User GetUserByEMail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.EMail == email);
        }

        public Models.User GetByePlayer()
        {
            return _context.Users.FirstOrDefault(u => u.Id == -1);
        }
    }
}