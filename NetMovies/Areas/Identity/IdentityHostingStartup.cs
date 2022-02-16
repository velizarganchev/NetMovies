using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(NetMovies.Areas.Identity.IdentityHostingStartup))]
namespace NetMovies.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}