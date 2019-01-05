using System;
using System.Collections.Generic;
using System.Linq;
using IDVDriver.BusinessContracts;
using IDVDriver.Domain;
using IDVDriver.Utils;

namespace IDVDriver.BusinessLogic
{
    public class UserService : IUserService
    {
        public string ConnectionString { get; set; }

        public bool CreateUser(User user)
        {
            var validator = new UserValidator();
            var results = validator.Validate(user);
            if (results.IsValid)
            {
                using (var context = new IDVContext(ConnectionString))
                {
                    context.Users.Add(user);
                    return context.SaveChanges() > 0;
                }
            }
            else
            {
                throw new BusinessException("Cannot create user", results.Errors);
            }
        }

        public bool UpdateUser(User user)
        {
            var validator = new UserValidator();
            var results = validator.Validate(user);
            if (results.IsValid)
            {
                using (var context = new IDVContext(ConnectionString))
                {
                    var incomeFromDb = context.Users.First(x => x.Id == user.Id);
                    //TODO: update when more variables are committed
                    var count = context.SaveChanges();
                    return count > 0;
                }
            }
            else
            {
                throw new BusinessException("Cannot update user", results.Errors);
            }          
        }

        public bool DeleteUser(Guid userId)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                var user = context.Users.First(x => x.Id == userId);
                context.Users.Remove(user);
                var count = context.SaveChanges();
                return count > 0;
            }
        }

        public User GetUser(Guid userId)
        {
            using (var context = new IDVContext(ConnectionString))
            {
                return context.Users.First(x => x.Id == userId);
            }
        }

        public List<User> GetUsers()
        {
            using (var context = new IDVContext(ConnectionString))
            {
                return context.Users.ToList();
            }
        }
    }
}