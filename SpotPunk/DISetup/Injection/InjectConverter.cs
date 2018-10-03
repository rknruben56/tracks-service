using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using SpotPunk.DISetup.Injection.Internal;

namespace SpotPunk.DISetup.Injection
{
    /// <summary>
    /// DISCLAIMER: Taken from https://github.com/ryanspletzer/DependencyInjectionAzureFunction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InjectConverter<T> : IConverter<Anonymous, T>
    {
        private readonly ServiceProvider _provider;

        public InjectConverter(ServiceProvider provider)
        {
            _provider = provider;
        }

        public T Convert(Anonymous input)
        {
            return _provider.GetService<T>();
        }
    }
}
