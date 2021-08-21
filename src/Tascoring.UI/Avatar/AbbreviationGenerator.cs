using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;

namespace Tascoring.UI.Avatar
{
	public class AbbreviationGenerator : IGenerateAvatar
	{

		private readonly IAvataratorConfig _config;
		public AbbreviationGenerator(IAvataratorConfig config)
		{
			_config = config;
		}

		public byte[] Generate(string text, int width, int height)
		{
			var abbreviation = GetAbbreviation(text);
			int index = abbreviation[0] % (_config.BackgroundColors.Count - 1);
			var backgroundColor = _config.BackgroundColors.ElementAt(index);
			using (var bitmap = new Bitmap(width, height))
			{
				using (var graphics = Graphics.FromImage(bitmap))
				{
					graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

					using (var brush = new SolidBrush(backgroundColor))
					{
						graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
					}

					StringFormat format = GetStringFormat();
					RectangleImage(width, height, out RectangleF rectangle, out double stringMaxHeight, out double stringMaxWidth);
					Font font = CalculateFont(abbreviation, stringMaxHeight, stringMaxWidth, graphics, new Font(FontFamily.GenericSansSerif, (float)height / 2, FontStyle.Bold));
					using (var brush = new SolidBrush(Color.White))
					{
						graphics.DrawString(abbreviation, font, brush, rectangle, format);
					}
				}
				byte[] data = GetImageAsBytes(bitmap);
				return data;
			}
		}

		private static byte[] GetImageAsBytes(Bitmap bitmap)
		{
			ImageConverter converter = new ImageConverter();
			return converter.ConvertTo(bitmap, typeof(byte[])) as byte[];
		}

		public bool Generate(string data, int width, int height, string path)
		{
			var abbreviation = GetAbbreviation(data);
			int index = abbreviation[0] % (_config.BackgroundColors.Count() - 1);
			var backgroundColor = _config.BackgroundColors.ElementAt(index);
			using (var bitmap = new Bitmap(width, height))
			{
				using (var graphics = Graphics.FromImage(bitmap))
				{
					graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

					using (var brush = new SolidBrush(backgroundColor))
					{
						graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
					}

					StringFormat format = GetStringFormat();
					RectangleImage(width, height, out RectangleF rectangle, out double stringMaxHeight, out double stringMaxWidth);
					Font font = CalculateFont(abbreviation, stringMaxHeight, stringMaxWidth, graphics, new Font(FontFamily.GenericSansSerif, (float)height / 2, FontStyle.Bold));
					using (var brush = new SolidBrush(Color.White))
					{
						graphics.DrawString(abbreviation, font, brush, rectangle, format);
						graphics.Save();
					}
				}

				try
				{
					bitmap.Save(path, ImageFormat.Jpeg);
					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		private static StringFormat GetStringFormat() => new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };

		private static void RectangleImage(int width, int height, out RectangleF rectangle, out double stringMaxHeight, out double stringMaxWidth)
		{
			rectangle = new RectangleF(0, 0, width, height);
			stringMaxHeight = height - height * 0.1;
			stringMaxWidth = height - height * 0.1;
		}

		private static Font CalculateFont(string text, double maxHeight, double maxWidth, Graphics graphics, Font actualFont)
		{
			var size = graphics.MeasureString(text, actualFont);
			if (size.Width > maxWidth || size.Height > maxHeight)
				return CalculateFont(text, maxHeight, maxWidth, graphics, new Font(actualFont.FontFamily, actualFont.Size - 0.1f, actualFont.Style));
			return actualFont;
		}
		private static string GetAbbreviation(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return "SA";

			input = input.Trim();
			if (input.Contains(' '))
			{
				var splited = input.Split(' ');
				return $"{splited[0].ToUpper()[0]}{splited[1].ToUpper()[0]}";
			}

			var pascalCase = input;
			pascalCase = input.ToUpper()[0] + pascalCase.Substring(1);
			var upperCaseOnly = string.Concat(pascalCase.Where(c => char.IsUpper(c)));
			if (upperCaseOnly.Length > 1 && upperCaseOnly.Length <= 3)
			{
				return upperCaseOnly.ToUpper();
			}

			if (input.Length <= 3)
				return input.ToUpper();
			return input.Substring(0, 3).ToUpper();
		}
	}
}
