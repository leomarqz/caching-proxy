
using Spectre.Console.Cli;

namespace caching_proxy.cli.Settings
{
    public class ProxyClearCacheCommandSettings : CommandSettings
    {
        [CommandOption("-c|--clear-cache")]
        public bool ClearCache { get; set; }
    }
}