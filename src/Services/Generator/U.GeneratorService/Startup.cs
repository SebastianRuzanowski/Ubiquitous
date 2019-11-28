﻿using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Consul;
using U.Common.Fabio;
using U.Common.Mvc;
using U.GeneratorService.BackgroundServices;
using U.GeneratorService.Services;
using U.GeneratorService.Services.Generator;

namespace U.GeneratorService
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc()
                .AddConsulServiceDiscovery()
                .AddHTTPServiceClient<ISmartStoreAdapter>("u.smartstore-adapter")
                .AddUpdateWorkerHostedService(Configuration)
                .AddCustomServices();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UsePathBase(Configuration, _logger).Item1
                .UseDeveloperExceptionPage()
                .UseServiceId()
                .UseForwardedHeaders()
                .UseMvc();

            var consulServiceId = app.UseConsulServiceDiscovery();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }

    public static class CustomExtensions
    {
        public static IServiceCollection AddUpdateWorkerHostedService(this IServiceCollection services, IConfiguration configuration)
        {
            var backgroundService = configuration.GetOptions<BackgroundServiceOptions>("backgroundService");
            services.AddSingleton(backgroundService);
            services.AddHostedService<FakeProductsGeneratorWorkerHostedService>();
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IProductGenerator, FakeProductGenerator>();
            return services;
        }
    }
}