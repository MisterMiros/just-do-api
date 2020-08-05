using API.Model.Context;
using API.Model.Utils;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model.Repositories
{
    [Repository]
    public class UserRepository : BaseRepository
    {
        public UserRepository(ApplicationDatabaseContext dbContext) : base(dbContext) {}

        public User Create(string email, string password)
        {
            User user = new User()
            {
                Email = email,
                PasswordHash = PasswordUtils.Hash(password)
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public User Find(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public User Find(int id)
        {
            return _dbContext.Users.Find(id);
        }
    }
}
