
using System;
using System.Threading;
using caching_proxy.cli.Settings;
using caching_proxy.core;
using caching_proxy.extras;
using Spectre.Console.Cli;

namespace caching_proxy.cli.Commands
{
    public class ProxyCommand : Command<ProxyCommandSettings>
    {
        public override int Execute(CommandContext context, ProxyCommandSettings settings, CancellationToken cancellationToken)
        {
            // Validaciones básicas
            if (settings.Port <= 1000 || settings.Port >= 65535 || string.IsNullOrWhiteSpace(settings.Origin))
            {
                new LogC("ProxyCommand.cs").Error(
                    "Invalid port number or origin URL. Ensure the port is between 1001 and 65534 and the origin URL is provided."
                );
                return 1;
            }

            var ttl = string.IsNullOrEmpty(settings.Ttl)
                ? TimeSpan.FromMinutes(5)
                : TTLParser.Parse(settings.Ttl);

            var config = new CacheProxyConfiguration
            {
                Port = settings.Port,
                Origin = settings.Origin,
                TTL = ttl
            };
            
            var cacheManager = new CacheManager();
            var proxyServer = new ProxyServer(config, cacheManager);

            proxyServer.Start();

            // Manejo de Ctrl+C y cancelación del proceso
            cancellationToken.Register(() =>
            {
                proxyServer.Stop();
            });

            //  Mantiene el proceso vivo mientras no se cancele
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(100);
            }

            return 0;

        }
    }
}