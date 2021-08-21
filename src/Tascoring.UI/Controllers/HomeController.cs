using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Tascoring.UI.Models;
using Tascoring.UI.Services.RoomService;

namespace Tascoring.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IRoomService _roomService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HomeController(ILogger<HomeController> logger, IRoomService roomService, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_roomService = roomService;
			_httpContextAccessor = httpContextAccessor;
		}

		public IActionResult Index()
		{
			_logger.LogInformation("Selamun Aleykum !");

			if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated == false)
			{
				ViewData["ErrorMessage"] = "You must sign-in with your windows username and password";
				return View();
			}

			HomeVM model = new()
			{
				Rooms = _roomService.Rooms,
			};

			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
