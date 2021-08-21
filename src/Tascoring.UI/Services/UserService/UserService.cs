using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Tascoring.UI.Avatar;
using Tascoring.UI.Extensions;
using static System.String;

namespace Tascoring.UI.Services.UserService
{
	public class UserService : IUserService, IUserAvatarService
	{
		private readonly IGenerateAvatar _generateAvatar;
		private readonly IWebHostEnvironment _env;
		public UserService(IGenerateAvatar generateAvatar, IWebHostEnvironment env)
		{
			_generateAvatar = generateAvatar;
			_env = env;
		}

		public int GenerateUserIdWithUsername(string username) => GenerateId(username);

		public async Task<int> GenerateUserIdWithUsernameAsync(string username)
		{
			var userId = await Task.Run(() => GenerateId(username)).ConfigureAwait(false);
			return userId;
		}
		public async Task<bool> CheckUserAvatarIsExistsAsync(int userId)
		{
			string fileName = GetFilePath(userId, _env.WebRootPath);
			var result = await Task.Run(() => File.Exists(fileName)).ConfigureAwait(false);
			return result;
		}

		private const int _width = 200;
		private const int _height = 200;
		public async Task<string> CreateAvatarAsync(string username, int? userId = null)
		{
			if (!userId.HasValue)
				userId = await GenerateUserIdWithUsernameAsync(username);

			var avatarDataAsBytes = _generateAvatar.Generate(username, _width, _height);
			var fileSavePath = GetFilePath(userId.Value, _env.WebRootPath);
			try
			{
				await fileSavePath.WriteFileAsBytesAsync(avatarDataAsBytes).ConfigureAwait(false);
				return fileSavePath;
			}
			catch
			{
				throw;
			}
		}

		protected static string GetFilePath(int userId, string wrPath)
		{
			var fileName = Concat(userId, ".jpg");
			var filePath = Path.Combine(wrPath, "avatar", fileName);
			return filePath;
		}

		protected static int GenerateId(string username)
		{
			if (IsNullOrWhiteSpace(username))
				return -1;

			int id = 0;
			for (int i = 0; i < username.Length; i++)
			{
				id += username[i] * byte.MaxValue;
			}
			return id;
		}
	}

}
