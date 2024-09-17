using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NATS.Client.Core;
using Tgs.MessageQueue.Nats.Serializers;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddTgsLog();
builder.WebHost.UseSentry();
var env = builder.Environment;
var services = builder.Services;

services
    .AddSecurity()
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
    .AddKeyedSingleton(
        "NatQueue",
        (sp, _) => new NatsConnection(NatsOpts.Default with
        {
            Url = builder.Configuration.GetConnectionString("Nats")!,
            SerializerRegistry = TgsNatsSerializerRegistration.Default
        }))
    .AddMessageQueue()
    .AddNatsMessageQueue(config =>
    {
        config.ConfigureResolveConnection(sp => sp.GetRequiredKeyedService<NatsConnection>("NatQueue"));
    })
    .AddNatsGlobPatternExchange("*"); ;

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


var app = builder.Build();
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

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Global API"));
app.MapHealthChecks("/health");

await app.RunAsync();