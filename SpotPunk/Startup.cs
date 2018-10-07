using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SpotPunk.Providers;
using SpotPunk.Services;

[assembly: WebJobsStartup(typeof(SpotPunk.Startup))]
namespace SpotPunk
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddTransient<ISearchTermProvider, SearchTermProvider>();
            builder.Services.AddTransient<IMusicService, SpotifyService>();
        }
    }
}
