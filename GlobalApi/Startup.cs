using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Tgs.Global
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddGlobalApi((options, sp) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    options.NatsUrl = config.GetConnectionString("Nats");
                })
                .UseTgsRepository((sp, builder) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();

                    builder.UseLazyLoadingProxies();

                    builder.UseNpgsql(
                        config.GetConnectionString("Tgs"),
                        serverOption =>
                        {
                            serverOption.SetPostgresVersion(12, 5);
                            serverOption.CommandTimeout(180);
                            serverOption.UseTrigrams();
                        });

                    builder.UseSnakeCaseNamingConvention();
                })
                .Services
                .AddNatsMessageQueue(config =>
                {
                    config.ConfigQueueOptions((options, sp) =>
                    {
                        var globalApiOptions = sp.GetRequiredService<IOptions<GlobalApiOptions>>().Value;
                        options.Url = globalApiOptions.NatsUrl;
                    });
                });

            services.AddRouting(options => options.LowercaseUrls = true)
                .AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GlobalApi",
                    Version = "v1"
                });

                var xml = "GlobalApi.xml";
                var path = Path.Combine(AppContext.BaseDirectory, xml);
                if (File.Exists(path))
                    c.IncludeXmlComments(path);
            });

            services
                .AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tgs Global Api v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}