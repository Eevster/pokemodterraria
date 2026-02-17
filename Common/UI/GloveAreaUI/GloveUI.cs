using Terraria;
using Terraria.UI;

namespace Pokemod.Common.UI.GloveAreaUI
{
    public class GloveUI : UIState
    {
        public override void OnInitialize()
        {
            var gloveElement = new GloveUIElement();
            gloveElement.Width.Set(Main.screenWidth, 0);
            gloveElement.Height.Set(Main.screenHeight, 0);
            Append(gloveElement);
        }
    }
}