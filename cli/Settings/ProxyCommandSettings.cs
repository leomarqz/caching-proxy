
using System.ComponentModel;
using Spectre.Console.Cli;

namespace caching_proxy.cli.Settings
{
    public class ProxyCommandSettings : CommandSettings
    {

        [CommandOption("-p|--port <PORT>")]
        [Description("Port where the proxy will listen (default: 8080).")]
        public int Port { get; set; } = 8080;

        [CommandOption("-o|--origin <URL>")]
        [Description("Origin server to which the proxy will forward requests.")]
        public string Origin { get; set; } = string.Empty;

        [CommandOption("-t|--ttl <time>")]
        [Description("Cache entry time-to-live (format: 5s, 5m, 5h, 5d, etc., optional).")]
        public string Ttl { get; set; } = string.Empty;
    }
}