using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _context.Users
                .Where(u =>
                    u.Id.Equals(id) &&
                    u.Active != false)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<User>> GetAllUsers()
        {
            return await _context.Users
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }
    }
}