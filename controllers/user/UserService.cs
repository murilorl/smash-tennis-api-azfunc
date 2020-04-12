using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

using App.Data;
using App.Data.Model;

namespace App.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IList<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.DominantHand)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsers(IDictionary<string, string> queryParams)
        {

            //FacebookId
            //Email
            return await _context.Users
                .Where(u =>
                    u.Email.Equals(queryParams["Email"]) &&
                    u.FacebookId.Equals(queryParams["FacebookId"]))
                .Include(u => u.DominantHand)
                .OrderBy(u => u.Created)
                .ToListAsync();
        }
        public async Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();

            var entity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<User> Update(Guid id, User user)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            User cUser = await GetUserById(id);

            if (cUser == null)
            {
                throw new ApplicationException(String.Format("User not found for Id {0}", id));
            }

            cUser.Updated = DateTime.Now;
            cUser.LastName = user.LastName;

            _context.Users.Update(cUser);
            await _context.SaveChangesAsync();

            return cUser;
        }

        public async Task UpdatePartial(Guid id, JsonPatchDocument<User> user)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            User cUser = await GetUserById(id);

            if (cUser == null)
            {
                throw new ApplicationException(String.Format("User not found for Id {0}", id));
            }

            user.ApplyTo(cUser);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            User cUser = await GetUserById(id);

            if (cUser == null)
            {
                throw new ApplicationException(String.Format("User not found for Id {0}", id));
            }

            cUser.Updated = DateTime.Now;
            cUser.Active = false;

            _context.Users.Update(cUser);
            await _context.SaveChangesAsync();

        }
    }
}