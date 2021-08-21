using System.Threading.Tasks;

namespace Tascoring.UI.Services.UserService
{
	public interface IUserService
	{
		int GenerateUserIdWithUsername(string username);
		Task<int> GenerateUserIdWithUsernameAsync(string username);		
	}
}