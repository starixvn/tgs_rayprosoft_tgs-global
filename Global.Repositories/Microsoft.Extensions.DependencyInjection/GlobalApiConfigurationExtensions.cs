// ReSharper disable CheckNamespace

using System;
using Microsoft.EntityFrameworkCore;
using Tgs.Global.Configuration;
using Tgs.Global.Repositories;
using Tgs.Global.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GlobalApiConfigurationExtensions
    {
        public static GlobalApiConfiguration UseTgsRepository(
            this GlobalApiConfiguration configuration,
            Action<IServiceProvider, DbContextOptionsBuilder> configure)
        {
            configuration.Services
                .AddDbContext<TgsContext>(configure);

            return configuration.AddTgsRepository<TgsRepository>();
        }
    }
}