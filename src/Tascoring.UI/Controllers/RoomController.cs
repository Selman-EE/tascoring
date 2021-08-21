using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tascoring.UI.Services.UserService;
using Tascoring.UI.Extensions;
using Tascoring.UI.Models;
using Microsoft.AspNetCore.SignalR;
using Tascoring.UI.Hubs;
using System.Collections.Generic;
using System.Linq;

namespace Tascoring.UI.Controllers
{
    public class RoomController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUserAvatarService _userAvatarService;
        private readonly IHubContext<ScoringHub> _hubContext;

        public RoomController(IHttpContextAccessor httpContextAccessor, IUserService userService, IUserAvatarService userAvatarService, IHubContext<ScoringHub> hubContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _userAvatarService = userAvatarService;
            _hubContext = hubContext;
        }

        private static readonly List<string> Users = new()
        {
            "selman.ekici",
            "hasan.ekici",
            "arifhan.ekici",
            "nimet.vural",
            "remzi.ekici",
            "hidayet.vural",
            "ibrahim.halilsoy",
            "semra.nas",
            "mehmet.cakmaz",
            "ali.evdi",
            "inan.yalcin",
        };

        [Route("{roomId}")]
        [Route("[controller]/{roomId}")]
        public async Task<IActionResult> Index([FromRoute] string roomId)
        {
            //SELMANEE\\See		
            //BEYMEN\\selman.ekici																							 d
            //var test2 = User.Identity.Name;
            //var userName = "selman.ekici";
            ////			
            var name = Users.First();
            Users.Remove(name);
            var winUserName = name;

            //var winUserName = _httpContextAccessor.HttpContext.User.Identity.Name.GetUserName();
            var displayName = winUserName.CreateDisplayName();
            var userId = await _userService.GenerateUserIdWithUsernameAsync(winUserName).ConfigureAwait(false);
            var userHasAvatar = await _userAvatarService.CheckUserAvatarIsExistsAsync(userId).ConfigureAwait(false);
            if (!userHasAvatar)
                _ = await _userAvatarService.CreateAvatarAsync(displayName, userId).ConfigureAwait(false);

            RoomVM model = new()
            {
                UserId = userId,
                RoomId = roomId,
                Username = winUserName,
                DisplayName = displayName
            };


            // send user to hub group					
            await _hubContext.Clients.Group(roomId).SendAsync("newPersonJoinedToRoom", model).ConfigureAwait(false);
            return View(model);
        }
    }
}
