using System;
using Microsoft.Extensions.DependencyInjection;

namespace Tgs.Global.Configuration
{
    public class GlobalApiConfiguration
    {
        public GlobalApiConfiguration(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }

        public GlobalApiConfiguration AddTgsRepository<TRepository>()
            where TRepository : class, ITgsRepository
        {
            Services.AddTransient<ITgsRepository, TRepository>();

            return this;
        }
    }
}