
using System;
using System.IO;

namespace caching_proxy.extras
{
    public static class Banner
    {
        public static void Show()
        {
            if(File.Exists("static/banner.txt"))
            {
                var banner = File.ReadAllText("static/banner.txt");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(banner);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("=== Caching Proxy ===");
            }
        }
    }
}