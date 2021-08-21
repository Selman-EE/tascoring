using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tascoring.UI.Hubs.Models;

namespace Tascoring.UI.Hubs
{
	//Chrome 'da hata ERR_SPDY_INADEQUATE_TRANSPORT_SECURITY alırsanız, geliştirme sertifikanızı güncelleştirmek için şu komutları çalıştırın:
	// dotnet dev-certs https --clean
	// dotnet dev-certs https --trust

	//https://docs.microsoft.com/tr-tr/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-javascript-client
	//https://docs.microsoft.com/en-us/aspnet/core/signalr/api-design?view=aspnetcore-5.0
	public partial class ScoringHub : Hub
	{
		private static readonly ConcurrentDictionary<string, List<Occupant>> _users = new();
		public async Task SendMessage(string name, string message)
		{
			await Clients.Caller.SendAsync("Whatever", "OK", name, message);
		}
		public override async Task OnConnectedAsync()
		{
			var userId = GetUserIdFromQuery(Context);
			var displayName = GetDisplayNameFromQuery(Context);
			var avatarUrl = GetAvatarUrlFromQuery(Context);
			var roomId = GetRoomIdFromQuery(Context); // Bağlantı sağlandığında oda aynı gruba alınıyor.			
			Occupant occupant = new()
			{
				UserId = userId,
				DisplayName = displayName,
				AvatarUrl = avatarUrl,
				ConnectionId = Context.ConnectionId,
			};
			AddUserToGroup(roomId, occupant);
			await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
			var users = GetUsersInGroup(roomId).Where(x => x.UserId != userId);
			if (users.Any())
				await Clients.Caller.SendAsync("AddOtherUsersInRoom", users);
			// TODO: puan verilerken son verilen puanlari _users icine ekle her zaman. Ve odaya yeni biri gelirse ona gore goster
			await base.OnConnectedAsync();
		}
		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var userId = GetUserIdFromQuery(Context);
			var displayName = GetDisplayNameFromQuery(Context);
			var avatarUrl = GetAvatarUrlFromQuery(Context);
			var roomId = GetRoomIdFromQuery(Context);
			Occupant occupant = new()
			{
				UserId = userId,
				DisplayName = displayName,
				AvatarUrl = avatarUrl,
				ConnectionId = Context.ConnectionId,
			};
			RevomeUserFromGroup(roomId, occupant);			
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
			await Clients.OthersInGroup(roomId).SendAsync("RemoveUsersInRoom", userId.ToString());
			await base.OnDisconnectedAsync(exception);
			//Dispose(true);
		}
		protected override void Dispose(bool disposing) => base.Dispose(disposing);

		// Static Methods
		private static List<Occupant> GetUsersInGroup(StringValues roomId)
		{
			return _users.TryGetValue(roomId, out List<Occupant> list) ? list : new List<Occupant>();
		}
		private static bool AddUserToGroup(StringValues roomId, Occupant occupant)
		{
			if (_users.TryGetValue(roomId, out List<Occupant> users))
			{
				var occupants = users.ToList();
				occupants.Add(occupant);
				return _users.TryUpdate(roomId, occupants, users);
			}
			else
			{
				var occupants = new List<Occupant>
				{
					occupant
				};
				return _users.TryAdd(roomId, occupants);
			}
		}
		private static bool RevomeUserFromGroup(StringValues roomId, Occupant occupant)
		{
			var removeUserFromOccupants = new List<Occupant>
			{
				occupant
			};
			return _users.TryRemove(roomId, out removeUserFromOccupants);
		}
		private static StringValues GetRoomIdFromQuery(HubCallerContext hubCallerContext)
		{
			return hubCallerContext.GetHttpContext().Request.Query["roomId"];
		}
		private static StringValues GetAvatarUrlFromQuery(HubCallerContext hubCallerContext)
		{
			return hubCallerContext.GetHttpContext().Request.Query["avatarUrl"];
		}
		private static StringValues GetDisplayNameFromQuery(HubCallerContext hubCallerContext)
		{
			return hubCallerContext.GetHttpContext().Request.Query["displayName"];
		}
		private static StringValues GetUserIdFromQuery(HubCallerContext hubCallerContext)
		{
			return hubCallerContext.GetHttpContext().Request.Query["user"];
		}
	}
}
