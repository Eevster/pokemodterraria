using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI
{
	internal class UIHelpers
	{
		public static void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
		{
			uiElement.Left.Set(left, 0f);
			uiElement.Top.Set(top, 0f);
			uiElement.Width.Set(width, 0f);
			uiElement.Height.Set(height, 0f);
		}

		public static void SetRectangleAlign(UIElement uiElement, float left, float top, float width, float height)
		{
			uiElement.HAlign = left;
			uiElement.VAlign = top;
			uiElement.Width.Set(width, 0f);
			uiElement.Height.Set(height, 0f);
		}

	}
}