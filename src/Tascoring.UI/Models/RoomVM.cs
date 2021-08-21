using System;

namespace Tascoring.UI.Models
{
	public class RoomVM
	{
		public int UserId { get; init; }
		public string RoomId { get; init; }
		public string DisplayName { get; init; }
		public string Username { get; init; }
		public string AvatarUrl
		{
			get
			{
				if (UserId > 0)
					return $"/avatar/{UserId}.jpg";
				else
					return $"/avatar/notfound/users-{new Random().Next(1, 16)}.jpg";

			}
		}
	}
}
