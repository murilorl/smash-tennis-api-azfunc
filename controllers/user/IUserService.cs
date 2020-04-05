using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using App.Data.Model;

namespace App.Service
{
    public interface IUserService { 
        Task<User> GetUserById(Guid id);
        Task<IList<User>> GetAllUsers();
    }
}