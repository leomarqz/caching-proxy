
using caching_proxy.cli.Commands;
using caching_proxy.extras;
using Spectre.Console.Cli;

namespace caching_proxy;

class Program
{
	public static int Main(string[] args)
    {
        Banner.Show();

        var app = new CommandApp();

        app.Configure((config) =>
        {
            config.SetApplicationName("caching-proxy");
            config.AddCommand<ProxyCommand>("start")
                  .WithDescription("Starts the caching proxy server");
        });

        return app.Run(args);
    }
}




