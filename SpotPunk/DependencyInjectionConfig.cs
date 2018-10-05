using Autofac;
using Indigo.Functions.Autofac;
using SpotPunk.Providers;
using SpotPunk.Services;

namespace SpotPunk
{
    /// <summary>
    /// AutoFac DI Config
    /// </summary>
    public class DependencyInjectionConfig : IDependencyConfig
    {
        public void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<SpotifyService>().As<IMusicService>();
            builder.RegisterType<SearchTermProvider>().As<ISearchTermProvider>();
        }
    }
}
