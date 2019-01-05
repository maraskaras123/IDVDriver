using System;
using System.Collections.Generic;
using IDVDriver.Domain;

namespace IDVDriver.BusinessContracts
{
    public interface IUserService
    {
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(Guid userId);
        User GetUser(Guid userId);
        List<User> GetUsers();
    }
}