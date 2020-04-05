using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using App.Data;
using App.Data.Model;

namespace App.Controllers
{
    public class UserController
    {

        public static async Task<User> GetUserById(AppDbContext context, string guid, HttpRequest req, ILogger log)
        {
            // TODO: Implement filters

            return await context.Users
            .Where(u =>
                u.Id.Equals(Guid.Parse(guid)) &&
                u.Active != false
            )
            .FirstOrDefaultAsync();
        }
        public static async Task<User> GetUserbyFacebookCredentials(AppDbContext context, User user, HttpRequest req, ILogger log)
        {
            return await context.Users
            .Where(u =>
                u.Active != false &&
                (u.FacebookId.Equals(user.FacebookId) ||
                u.Email.Equals(user.Email))
            )
            .FirstOrDefaultAsync();
        }
        public static async Task<List<User>> GetAllUsers(AppDbContext context, HttpRequest req, ILogger log)
        {
            // TODO: Implement filters

            return await context.Users
            .OrderBy(a => a.FirstName)
            .ToListAsync();
        }

        public static User NewInstance(User data)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = data.FirstName,
                Email = data.Email
            };
        }


    }
}