using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using U.Common.NetCore.Fabio;
using U.Common.NetCore.Mvc;

namespace U.Common.NetCore.Consul
{
    public static class Extensions
    {
        private const string ConsulSectionName = "consul";
        private const string FabioSectionName = "fabio";

        public static IServiceCollection AddConsulServiceDiscovery(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<ConsulOptions>(ConsulSectionName);
            services.Configure<ConsulOptions>(configuration.GetSection(ConsulSectionName));
            services.Configure<FabioOptions>(configuration.GetSection(FabioSectionName));


            services.AddTransient<IConsulServicesRegistry, ConsulServicesRegistry>();
            services.AddTransient<ConsulServiceDiscoveryMessageHandler>();

            services.AddTransient<IConsulIdentifierService>(
                c => new ConsulIdentifierIdentifierService(
                    c.GetService<ISelfInfoService>()));

            return services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(options.Url))
                {
                    cfg.Address = new Uri(options.Url);
                }
            }));
        }

        //Returns unique identifierService ID used for removing the identifierService from registry.
        public static string UseConsulServiceDiscovery(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var consulOptions = scope.ServiceProvider.GetService<IOptions<ConsulOptions>>();
                var fabioOptions = scope.ServiceProvider.GetService<IOptions<FabioOptions>>();

                var address = consulOptions.Value.Address;
                if (string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Consul address can not be empty.",
                        nameof(consulOptions.Value.PingEndpoint));
                }

                var uniqueId = scope.ServiceProvider.GetService<ISelfInfoService>().Id;
                var client = scope.ServiceProvider.GetService<IConsulClient>();
                var serviceName = consulOptions.Value.Service;
                var serviceId = $"{serviceName}:{uniqueId}";
                var port = consulOptions.Value.Port;
                var pingEndpoint = consulOptions.Value.PingEndpoint;
                var pingInterval = consulOptions.Value.PingInterval <= 0 ? 5 : consulOptions.Value.PingInterval;
                var removeAfterInterval =
                    consulOptions.Value.RemoveAfterInterval <= 0 ? 10 : consulOptions.Value.RemoveAfterInterval;
                var registration = new AgentServiceRegistration
                {
                    Name = serviceName,
                    ID = serviceId,
                    Address = address,
                    Port = port,
                    Tags = GetFabioTags(serviceName, fabioOptions.Value.Service)
                };
                if (consulOptions.Value.PingEnabled)
                {
                    var scheme = address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                        ? string.Empty
                        : "http://";
                    var check = new AgentServiceCheck
                    {
                        Interval = TimeSpan.FromSeconds(pingInterval),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(removeAfterInterval),
                        HTTP = $"{scheme}{address}{(port > 0 ? $":{port}" : string.Empty)}/{pingEndpoint}/{uniqueId}"
                    };
                    registration.Checks = new[] {check};
                }

                client.Agent.ServiceRegister(registration);

                return serviceId;
            }
        }

        private static string[] GetFabioTags(string consulService, string fabioService)
        {
            var service = (string.IsNullOrWhiteSpace(fabioService) ? consulService : fabioService).ToLowerInvariant();

            return new[] {$"urlprefix-/{service} strip=/{service}"};
        }
    }
}