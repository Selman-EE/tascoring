using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Tascoring.UI.Services.Middleware
{
	public class UnhandledExceptionMiddleware
	{
		private readonly ILogger _logger;
		private readonly RequestDelegate _next;

		public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware> logger, RequestDelegate next)
		{
			this._logger = logger;
			this._next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception,
					$"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");
			}
		}
	}
}
