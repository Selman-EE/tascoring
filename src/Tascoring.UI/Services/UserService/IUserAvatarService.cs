using System.Threading.Tasks;

namespace Tascoring.UI.Services.UserService
{
	public interface IUserAvatarService
	{
		Task<string> CreateAvatarAsync(string username, int? userId = null);
		Task<bool> CheckUserAvatarIsExistsAsync(int userId);
	}
}
