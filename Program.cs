using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OpenFIS
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).ConfigureAppConfiguration((hostContext, builder) => { if (hostContext.HostingEnvironment.IsDevelopment()) builder.AddUserSecrets<Program>(); }).Build().Run();
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(x => x.UseStartup<Startup>());
    }
}
