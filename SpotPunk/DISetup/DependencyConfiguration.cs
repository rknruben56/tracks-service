using Microsoft.Extensions.DependencyInjection;
using SpotPunk.DISetup.Injection;
using SpotPunk.Providers;
using SpotPunk.Services;

namespace SpotPunk.DISetup
{
    /// <summary>
    /// DISCLAIMER: Taken from https://github.com/ryanspletzer/DependencyInjectionAzureFunction
    /// </summary>
    public class DependencyConfiguration : IDependencyConfiguration
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Injecting some stuff...
            services.AddSingleton<ISearchTermProvider, SearchTermProvider>();
            services.AddSingleton<IMusicService, SpotifyService>();
        }
    }
}
