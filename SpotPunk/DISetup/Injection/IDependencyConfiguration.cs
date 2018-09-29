using Microsoft.Extensions.DependencyInjection;

namespace SpotPunk.DISetup.Injection
{
    /// <summary>
    /// DISCLAIMER: Taken from https://github.com/ryanspletzer/DependencyInjectionAzureFunction
    /// </summary>
    public interface IDependencyConfiguration
    {
        #region Methods

        void ConfigureServices(IServiceCollection services);
        
        #endregion
    }
}
