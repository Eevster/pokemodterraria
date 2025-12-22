using Terraria;
using Terraria.UI;

namespace Pokemod.Common.UI.PokemonCursorUI
{
    public class CursorUI : UIState
    {
        public override void OnInitialize()
        {
            var cursorElement = new CursorUIElement();
            cursorElement.Width.Set(Main.screenWidth, 0);
            cursorElement.Height.Set(Main.screenHeight, 0);
            Append(cursorElement);
        }
    }
}