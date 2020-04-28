using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.JsonPatch;

using App.Data.Model.Users;

namespace App.Service
{
    public interface IUserService
    {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserById(Guid id, bool includeInactive);
        Task<IList<User>> GetAllUsers();
        Task<List<User>> GetAllUsers(IDictionary<string, string> queryParams);
        Task<User> Create(User user);
        Task<User> Update(Guid id, User user);
        Task UpdatePartial(Guid id, JsonPatchDocument<User> user);
        Task Delete(Guid id);
        Task<bool> IsEmailAvailable(String email);
        Task<User> SignInWithFacebook(User user);
        Task<User> SignInWithBasicAuth(User user);
    }
}