
using System.Threading;
using caching_proxy.cli.Settings;
using Spectre.Console.Cli;

namespace caching_proxy.cli.Commands
{
    public class ProxyClearCacheCommand : Command<ProxyClearCacheCommandSettings>
    {
        public override int Execute(CommandContext context, ProxyClearCacheCommandSettings settings, CancellationToken cancellationToken)
        {
            if (settings.ClearCache)
            {
                // Logic to clear the cache
            }

            return 0;
        }
    }
}