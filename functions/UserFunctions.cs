using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [FunctionName("UsersRead")]
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

        [FunctionName("UsersCreate")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log)
        {
            IActionResult returnValue = null;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonSerializer.Deserialize<User>(requestBody);

            try
            {
                returnValue = new CreatedObjectResult(await _userService.Create(user));
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when tried to create user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("An exception occurred when tried to create user. Message: {0}", e));
            }
            return returnValue;
        }

        [FunctionName("UsersUpdate")]
        public async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "users/{guid}")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log,
            Guid guid)
        {
            IActionResult returnValue = null;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonSerializer.Deserialize<User>(requestBody);

            try
            {
                await _userService.Update(guid, user);
                returnValue = new NoContentResult();
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when tried to update user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("An exception occurred when tried to update user. Message: {0}", e));
            }
            return returnValue;
        }
        [FunctionName("UsersPartialUpdate")]
        public async Task<IActionResult> PartialUpdate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "users/{guid}")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log,
            Guid guid)
        {
            IActionResult returnValue = null;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            JsonPatchDocument<User> patchUser = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<User>>(requestBody);

            if (patchUser == null)
            {
                return new BadRequestObjectResult("The body of the request cannot be null");
            }

            try
            {
                await _userService.UpdatePartial(guid, patchUser);
                returnValue = new NoContentResult();
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when partially updating user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("An exception occurred when partially updating user. Message: {0}", e));
            }

            return returnValue;
        }

        [FunctionName("UsersDelete")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{guid}")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log,
            Guid guid)
        {
            IActionResult returnValue = null;

            try
            {
                await _userService.Delete(guid);
                returnValue = new OkResult();
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when deleting user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("An exception occurred when deleting user. Message: {0}", e));
            }
            return returnValue;
        }
    }
}