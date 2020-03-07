using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace App.Functions
{

    public class UserFunctions
    {

        private readonly AppDbContext _dbContext;

        public UserFunctions(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("GetUsers")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{guid?}")]
            HttpRequest req,
            ILogger log,
            string guid)
        {
            if (guid == null)
            {
                return new OkObjectResult(await GetAllUsers(req, log));
            }
            else
            {
                var user = await GetUserByGuid(guid, req, log);
                if (user != null)
                {
                    return new OkObjectResult(user);
                }

                return new NotFoundResult();
            }
        }

        private async Task<List<User>> GetAllUsers(HttpRequest req, ILogger log)
        {
            // TODO: Implement filters

            return await _dbContext.Users
            .OrderBy(a => a.firstName)
            .ToListAsync();
        }

        private async Task<User> GetUserByGuid(string guid, HttpRequest req, ILogger log)
        {
            // TODO: Implement filters

            return await _dbContext.Users
            .Where(a => a.guid.Equals(Guid.Parse(guid)))
            .FirstOrDefaultAsync();
        }
    }

}