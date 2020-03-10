using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using App.Data;
using App.Data.Model;
using App.Controllers;

namespace App.Functions
{

    public class UserFunctions
    {

        private readonly AppDbContext _context;

        public UserFunctions(AppDbContext dbContext)
        {
            _context = dbContext;
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
                // return new OkObjectResult(await GetAllUsers(req, log));
                return new OkObjectResult(await UserController.GetAllUsers(_context, req, log));
            }
            else
            {
                var user = await UserController.GetUserById(_context, guid, req, log);
                if (user != null)
                {
                    return new OkObjectResult(user);
                }

                return new NotFoundResult();
            }
        }

        [FunctionName("CreateUser")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users")]
                        HttpRequest req,
                        CancellationToken cts,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // requestBody = Regex.Replace(requestBody, @"\t|\n|\r", "");

            User data = JsonConvert.DeserializeObject<User>(requestBody);
            User user = UserController.NewInstance(data);

            var entity = await _context.Users.AddAsync(user, cts);
            await _context.SaveChangesAsync(cts);
            return new OkObjectResult(JsonConvert.SerializeObject(entity.Entity));
        }

        /*         private async Task<List<User>> GetAllUsers(HttpRequest req, ILogger log)
                {
                    // TODO: Implement filters

                    return await _dbContext.Users
                    .OrderBy(a => a.FirstName)
                    .ToListAsync();
                }

                private async Task<User> GetUserByGuid(string guid, HttpRequest req, ILogger log)
                {
                    // TODO: Implement filters

                    return await _dbContext.Users
                    .Where(a => a.Id.Equals(Guid.Parse(guid)))
                    .FirstOrDefaultAsync();
                } */
    }

}