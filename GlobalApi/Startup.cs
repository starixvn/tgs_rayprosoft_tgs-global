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
                .AddBrandCertificateClient((options, sp) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();

                    options.BrandCertificateBaseUrl = config.GetValue<Uri>("Global:BrandCertificateBaseUrl");
                })
                .AddGlobalApi((options, sp) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    options.NatsUrl = config.GetConnectionString("Nats");
                })
                .UseTgsRepository((sp, builder) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();

                    builder.UseLazyLoadingProxies();

                    builder.UseNpgsql(config.GetConnectionString("Tgs"));
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
                })
                .AddSecurity();

            services.AddRouting(options => options.LowercaseUrls = true)
                .AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
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
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x =>
                x.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());

            app.UseSwaggerUI();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
