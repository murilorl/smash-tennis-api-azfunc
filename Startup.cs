using System.IO;
using System.Reflection;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Core.Configuration;
using App.Data;
using App.Service;

[assembly: FunctionsStartup(typeof(Core.Startup))]
namespace Core
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<ConfigurationItems>(config.GetSection("ConfigurationItems"));

            builder.Services.AddDbContext<AppDbContext>(
               options => options.UseSqlServer(config.GetSection("ConfigurationItems")["SqlConnectionString"]));
            // builder.Services.AddDbContext<AppDbContext>();

            builder.Services.AddMvcCore()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddOptions();
        }
    }
}