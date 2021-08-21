namespace Tascoring.UI.Hubs.Models
{
	public class Occupant
	{
		public string ConnectionId { get; init; }
		public string UserId { get; init; }
		public string DisplayName { get; init; }
		public string AvatarUrl { get; init; }
		public int Score { get; set; }
		public bool IsCompleted { get; set; }
	}
}
