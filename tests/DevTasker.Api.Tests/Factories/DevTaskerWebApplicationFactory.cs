using DevTasker.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevTasker.Api.Tests.Factories;
public class DevTaskerWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(d => d.ServiceType == typeof(DevTaskerDbContext)
             || d.ServiceType == typeof(DbContextOptions<DevTaskerDbContext>)
             || (d.ServiceType.IsGenericType &&
                 d.ServiceType.GenericTypeArguments.Contains(typeof(DevTaskerDbContext))))
                .ToList();

            foreach (var d in descriptors)
                services.Remove(d);

            services.AddDbContext<DevTaskerDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        });
    }
}
