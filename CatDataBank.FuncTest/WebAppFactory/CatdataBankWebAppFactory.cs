using System.Net.Http;
using System.Threading.Tasks;
using CatDataBank.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;

namespace CatDataBank.FuncTest.WebAppFactory
{
    public class CatdataBankWebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });
        }
    }
}