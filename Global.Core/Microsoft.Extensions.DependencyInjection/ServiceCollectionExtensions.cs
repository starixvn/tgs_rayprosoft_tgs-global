// ReSharper disable CheckNamespace

using System;
using System.Reflection;
using Tgs.Global;
using Tgs.Global.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static GlobalApiConfiguration AddGlobalApi(
            this IServiceCollection services,
            Action<GlobalApiOptions, IServiceProvider> configure)
        {
            services.RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient);

            services.AddOptions<GlobalApiOptions>()
                .Configure(configure);

            return new GlobalApiConfiguration(services);
        }
    }
}