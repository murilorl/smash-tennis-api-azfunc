using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using Core.Configuration;

namespace FunctionApp1
{
    public  class Function1
    {
        private readonly ConfigurationItems _configurationItems;

        public Function1(IOptions<ConfigurationItems> configurationItems)
        {
            _configurationItems = configurationItems.Value;
        }


        [FunctionName("Function1")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req
            )
        {
            string localSettings = Environment.GetEnvironmentVariable("LocalSettingValue"); // Included so as to demo regular approach
            string commonValue = _configurationItems.CommonValue;
            string secretValue = _configurationItems.SecretValue;

            return new OkObjectResult($"Local Value : '{localSettings}' | Common Value : '{commonValue}' | Secret Value : '{secretValue}'");
        }
    }
}
