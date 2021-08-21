using System.Collections.Generic;
using System.Drawing;

namespace Tascoring.UI.Avatar
{
	public interface IAvataratorConfig
	{
		ICollection<Color> BackgroundColors { get; }

		IAvataratorConfig WithBackgroundColors(List<Color> colors);
	}
}
