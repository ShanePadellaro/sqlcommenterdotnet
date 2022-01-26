using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace TestNetCore31
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel((context, options) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        options.ListenLocalhost(56987, o => o.Protocols = HttpProtocols.Http1AndHttp2);
                        options.ListenLocalhost(50052, o => o.Protocols = HttpProtocols.Http2);
                    }
                    else
                    {
                        options.ListenAnyIP(80, o => o.Protocols = HttpProtocols.Http1AndHttp2);
                        options.ListenAnyIP(50051, o => o.Protocols = HttpProtocols.Http2);
                    }
                })
                .UseStartup<Startup>();
    }
}