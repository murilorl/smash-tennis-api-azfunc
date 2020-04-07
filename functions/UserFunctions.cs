using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using App.Data.Model;
using App.Functions.Responses;
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
            IDictionary<string, string> queryParams = req.GetQueryParameterDictionary();

            /*
            Prioriy search order is 1) GUID present in the URI, 2) parameters provided in the query string and lastly
            3) all resources with not filter if both 1) and 2) have not been provided  
            */
            if (guid != null)
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
            else if (queryParams.Count > 0)
            {
                return new OkObjectResult(await _userService.GetAllUsers(queryParams));
            }
            else
            {
                return new OkObjectResult(await _userService.GetAllUsers());
            }
        }

        [FunctionName("CreateUser")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log)
        {
            IActionResult returnValue = null;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // User user = JsonConvert.DeserializeObject<User>(requestBody);
            User user = JsonSerializer.Deserialize<User>(requestBody);

            try
            {
                returnValue = new CreatedObjectResult(await _userService.Create(user));
            }
            catch (Exception e)
            {
                log.LogError($"An exception occurred when tried to create a user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("An exception occurred when tried to create a user. Message: {0}", e));
            }
            return returnValue;
        }
    }
}