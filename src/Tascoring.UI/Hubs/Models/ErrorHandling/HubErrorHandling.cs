using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Tascoring.UI.Hubs.Models.ErrorHandling
{
	public class HubErrorHandling : IHubFilter
	{
		private readonly ILogger<ScoringHub> _logger;
		public HubErrorHandling(ILogger<ScoringHub> logger)
		{
			_logger = logger;
		}
		public async ValueTask<object> InvokeMethodAsync(
		HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
		{
			_logger.LogInformation($"Calling hub method '{invocationContext.HubMethodName}'");
			try
			{
				return await next(invocationContext);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Exception calling '{invocationContext.HubMethodName}': {ex}");
				throw;
			}
		}

		// Optional method
		public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
		{
			try
			{
				return next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Exception: {ex}");
				throw;
			}
		}

		// Optional method
		public Task OnDisconnectedAsync(
			HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
		{
			try
			{
				return next(context, exception);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Exception: {ex}");
				throw;
			}
		}
	}
}
