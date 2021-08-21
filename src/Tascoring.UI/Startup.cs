using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tascoring.UI.Services.RoomService;
using Tascoring.UI.Hubs;
using Tascoring.UI.Services.UserService;
using Tascoring.UI.Avatar;
using System;
using Microsoft.AspNetCore.Http.Connections;
using Tascoring.UI.Hubs.Models.ErrorHandling;
using Microsoft.AspNetCore.SignalR;
using Tascoring.UI.Services.Middleware;

namespace Tascoring.UI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(IISDefaults.AuthenticationScheme).AddNegotiate();
			services.AddControllersWithViews()
				.AddJsonOptions(options =>
				{
					//when response will see pretty
					//options.JsonSerializerOptions.WriteIndented = true;
					options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
					// Use the property (Pascal) casing . Default is camelCase 					
					//options.JsonSerializerOptions.PropertyNamingPolicy = null;
				});
			services.AddHttpContextAccessor();
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					.SetIsOriginAllowed((hosts) => true));
			});
			services.AddSignalR(options =>
			{
				options.EnableDetailedErrors = true;
				options.MaximumReceiveMessageSize = long.MaxValue;
				options.StreamBufferCapacity = 30;
				options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
				options.HandshakeTimeout = TimeSpan.FromSeconds(30);
				options.KeepAliveInterval = TimeSpan.FromSeconds(30);
				options.AddFilter<HubErrorHandling>();
			});

			RoomService roomService = new();
			Configuration.GetSection(nameof(RoomService)).Bind(roomService);
			services.AddSingleton<IRoomService>(roomService);
			services.AddSingleton<HubErrorHandling>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IUserAvatarService, UserService>();
			services.AddScoped<IAvataratorConfig, AvataratorConfig>();
			services.AddScoped<IGenerateAvatar, AbbreviationGenerator>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseMiddleware<UnhandledExceptionMiddleware>();

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapHub<ScoringHub>("/scoring-hub", options =>
				{
					options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
					options.TransportMaxBufferSize = long.MaxValue;
					options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
					options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(30);
				});
			});
		}
	}
}
