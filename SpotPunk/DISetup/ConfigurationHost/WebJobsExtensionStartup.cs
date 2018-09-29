using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using SpotPunk.DISetup.ConfigurationHost;

[assembly: WebJobsStartup(typeof(WebJobsExtensionStartup), "Azure Function Extension")]

namespace SpotPunk.DISetup.ConfigurationHost
{
    /// <summary>
    /// DISCLAIMER: Taken from https://github.com/ryanspletzer/DependencyInjectionAzureFunction
    /// </summary>
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection();
        }
    }
}
