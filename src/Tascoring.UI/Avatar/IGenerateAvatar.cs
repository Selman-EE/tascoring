namespace Tascoring.UI.Avatar
{
	public interface IGenerateAvatar
	{
		byte[] Generate(string text, int width, int height);
		bool Generate(string data, int width, int height, string path);
	}
}