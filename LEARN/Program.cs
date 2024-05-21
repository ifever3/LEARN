using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace LEARN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            var webHost = CreateWebHostBuilder(args).Build();
            webHost.Run();

        }

        private static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); });
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
