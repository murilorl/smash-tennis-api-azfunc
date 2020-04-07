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
                .Where(u =>
                    u.Active == true)
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
                    u.Active == true &&
                    u.Email.Equals(queryParams["Email"]) &&
                    u.FacebookId.Equals(queryParams["FacebookId"]))
                .Include(u => u.DominantHand)
                .OrderBy(u => u.Created)
                .ToListAsync();
            /*           string sqlProjection = "SELECT * FROM Users";
                      List<string> sqlPredicate = null;
                      foreach (KeyValuePair<string, string> kvp in queryParams)
                      {
                          if (sqlPredicate == null)
                          {
                              sqlPredicate = new List<string>();
                          }
                          sqlPredicate.Add(String.Format("u.{0} == {'1'}", kvp.Key, kvp.Value));
                      }

                      if (sqlPredicate != null)
                      {
                          sqlProjection = sqlProjection + " WHERE " + String.Join(" AND", sqlPredicate);
                      }

                      var users = _context.Users
                          .FromSqlRaw(sqlProjection)
                          .ToListAsync();

                      return users; */
        }
        public async Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();

            var entity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }
    }
}