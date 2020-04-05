using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using App.Service;

namespace App.Functions
{
    public class UserFunctions
    {
        private readonly IUserService _userService;
        public UserFunctions(IUserService userService)
        {
            _userService = userService;
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
                return new OkObjectResult(await _userService.GetAllUsers());
            }
            else
            {
                var user = await _userService.GetUserById(Guid.Parse(guid));
                if (user != null)
                {
                    return new OkObjectResult(user);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
        }
    }
}