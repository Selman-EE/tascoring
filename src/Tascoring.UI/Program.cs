using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Tascoring.UI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				 .ConfigureLogging((hostingContext, builder) =>
				 {
					 builder.ClearProviders();
					 builder.SetMinimumLevel(LogLevel.Error);
					 //builder.AddFile("Logs/app-{Date}.json", isJson: true);
				 })
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				}).Build();

			await host.RunAsync();
		}
	}
}
