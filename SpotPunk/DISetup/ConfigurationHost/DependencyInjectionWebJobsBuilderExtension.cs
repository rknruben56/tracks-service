using Microsoft.Azure.WebJobs;
using SpotPunk.DISetup.Injection;

namespace SpotPunk.DISetup.ConfigurationHost
{
    /// <summary>
    /// DISCLAIMER: Taken from https://github.com/ryanspletzer/DependencyInjectionAzureFunction
    /// </summary>
    public static class DependencyInjectionWebJobsBuilderExtension
    { 
        public static IWebJobsBuilder AddDependencyInjection(this IWebJobsBuilder builder)
        {
            builder.AddExtension(new InjectWebJobsExtension());
            return builder;
        }
    }
}
